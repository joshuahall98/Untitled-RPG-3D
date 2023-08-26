using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.ChangeAnimationState(AIController.AnimState.Attack, 0.1f, 0);
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
        
    }

    public override void UpdateState(AIStateManager state)
    {
        CheckAnimationFinished(state);
    }

    public override void ExitState(AIStateManager state)
    {
        //this has to be called after otherwise animator bugs
        controller.agent.isStopped = false;
    }

    void CheckAnimationFinished(AIStateManager state)
    {
        if (controller.IsAnimationDone(controller.anim, AIController.AnimState.Attack))
        {
            state.SwitchToTheNextState(state.IdleState);
        }
    }
    
}
