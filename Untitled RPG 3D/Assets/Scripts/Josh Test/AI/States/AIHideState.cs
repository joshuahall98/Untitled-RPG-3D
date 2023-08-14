using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHideState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.anim.SetBool("isHiding", true);
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
    }

    public override void UpdateState(AIStateManager state)
    {
        
    }

    public override void ExitState(AIStateManager state)
    {
        controller.anim.SetBool("isHiding", false);
        controller.agent.isStopped = false;
    }

   
}
