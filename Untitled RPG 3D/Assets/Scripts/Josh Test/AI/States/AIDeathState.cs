using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.ChangeAnimationState(AIController.AnimState.Death, 0f, 0);
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
        controller.GetComponent<CapsuleCollider>().enabled = false;
        controller.RotateToPlayer(100);

    }

    public override void UpdateState(AIStateManager state)
    {

    }

    public override void ExitState(AIStateManager state)
    {

    }

}
