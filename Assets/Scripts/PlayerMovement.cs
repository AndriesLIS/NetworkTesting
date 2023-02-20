using Mirror;
using UnityEngine;
using UnityEngine.AI;

public partial class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private float groundedRadius = 0.28f;
    [SerializeField] private LayerMask groundLayers;

    [Space(10)]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -15.0f;

    [HideInInspector] public AnimationManager animationM;
    [HideInInspector] public Animator animator;

    private bool isGrounded;

    private Camera mainCam;
    private Vector3 Velocity;

    private readonly int _animIDSpeed = Animator.StringToHash("Speed");
    private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    private readonly int _animIDJump = Animator.StringToHash("Jump");
    private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
    private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    public override void OnStartAuthority()
    {
        mainCam = Camera.main;

        animationM.Init();
    }

    private void Update()
    {
        if (!isOwned)
            return;

        if (animationM == null)
            return;

        GroundCheck();
        SetWalkSpeed();
        HandleJumping();
        TargetGetPos();
    }
    
    private void TargetGetPos()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
            return;

        if (NavMesh.SamplePosition(hit.point, out var hit2, 1, NavMesh.AllAreas))
            agent.SetDestination(hit.point);
    }

    [Client]
    private void HandleJumping()
    {
        if (isGrounded)
        {
            animationM.Cmd_SetJumpValue(false);
            animationM.Cmd_SetFreeFallValue(false);

            animator.SetBool(_animIDJump, false);
            animator.SetBool(_animIDFreeFall, false);

            if (Velocity.y < 0.0f)
                Velocity.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                animationM.Cmd_SetJumpValue(true);
                animator.SetBool(_animIDJump, true);
            }
        }
        else
        {
            animationM.Cmd_SetFreeFallValue(true);
            animator.SetBool(_animIDFreeFall, true);
        }
    }

    [Client]
    public void GroundCheck()
    {
        Vector3 spherePosition = new(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z);

        bool isGrounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore);

        animationM.Cmd_SetIsGrounded(true);
        animator.SetBool(_animIDGrounded, isGrounded);

        this.isGrounded = isGrounded;
    }

    [Client]
    public void SetWalkSpeed()
    {
        animationM.Cmd_SetAnimationBlend(agent.velocity.magnitude);
        animationM.Cmd_SetInputMagnitude(agent.velocity.normalized.magnitude);

        animator.SetFloat(_animIDSpeed, agent.velocity.magnitude);
        animator.SetFloat(_animIDMotionSpeed, agent.velocity.normalized.magnitude);
    }
}
