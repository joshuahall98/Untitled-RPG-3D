using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHideState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.ChangeAnimationState(AIController.AnimState.Hide, 0.1f, 0);
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
    }

    public override void UpdateState(AIStateManager state)
    {
        
    }

    public override void ExitState(AIStateManager state)
    {
        controller.agent.isStopped = false;
    }

   
}
