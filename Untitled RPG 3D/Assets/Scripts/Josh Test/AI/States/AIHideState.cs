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
    }

    public override void UpdateState(AIStateManager state)
    {
        
    }

    public override void ExitState(AIStateManager state)
    {
        controller.anim.SetBool("isHiding", false);
    }

   
}
