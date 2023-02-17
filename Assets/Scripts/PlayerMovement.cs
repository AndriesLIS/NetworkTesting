using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public NetworkAnimator animator;

    private Camera mainCam;

    public override void OnStartAuthority()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!isOwned)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TargetGetPos();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    
    private void TargetGetPos()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
            return;

        if (NavMesh.SamplePosition(hit.point, out var hit2, 1, NavMesh.AllAreas))
            agent.SetDestination(hit.point);
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
