using Mirror;
using UnityEngine;

public class AnimationManager : NetworkBehaviour
{
    [Header("Animation")]
    [Tooltip("Cant be assigned through editor or via code.")]
    [SerializeField] private Animator animator = default;

    // animation IDs
    private readonly int _animIDSpeed = Animator.StringToHash("Speed");
    private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    private readonly int _animIDJump = Animator.StringToHash("Jump");
    private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
    private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    //SyncVars
    [SyncVar(hook = nameof(OnIsGroundedChanged))]
    private bool isGrounded;
    [SyncVar(hook = nameof(OnAnimationBlendChanged))]
    private float animationBlend;
    [SyncVar(hook = nameof(OnJumpValueChanged))]
    private bool jumpValue;
    [SyncVar(hook = nameof(OnFreeFallValueChanged))]
    private bool freeFallValue;
    [SyncVar(hook = nameof(OnInputMagnitudeChanged))]
    private float inputMagnitude;

    #region hooks
    private void OnIsGroundedChanged(bool _, bool newValue)
    {
        animator.SetBool(_animIDGrounded, isGrounded);
    }

    private void OnAnimationBlendChanged(float _, float newValue)
    {
        animator.SetFloat(_animIDSpeed, animationBlend);
    }

    private void OnJumpValueChanged(bool _, bool newValue)
    {
        animator.SetBool(_animIDJump, jumpValue);
    }

    private void OnFreeFallValueChanged(bool _, bool newValue)
    {
        animator.SetBool(_animIDFreeFall, freeFallValue);
    }

    private void OnInputMagnitudeChanged(float _, float newValue)
    {
        animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

    [Command]
    public void Cmd_SetIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    [Command]
    public void Cmd_SetAnimationBlend(float animationBlend)
    {
        this.animationBlend = animationBlend;
    }

    [Command]
    public void Cmd_SetFreeFallValue(bool freeFallValue)
    {
        this.freeFallValue = freeFallValue;
    }

    [Command]
    public void Cmd_SetJumpValue(bool jumpValue)
    {
        this.jumpValue = jumpValue;
    }

    [Command]
    public void Cmd_SetInputMagnitude(float inputMagnitude)
    {
        this.inputMagnitude = inputMagnitude;
    }
    #endregion

    public void Init()
    {
        animator.SetBool(_animIDGrounded, isGrounded);
        animator.SetFloat(_animIDSpeed, animationBlend);
        animator.SetBool(_animIDJump, jumpValue);
        animator.SetBool(_animIDFreeFall, freeFallValue);
        animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }
}
