using System.Collections;
using System.Collections.Generic;
using System.Net.PeerToPeer.Collaboration;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class AIStaggerState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.agent.isStopped = true;
        controller.RotateToPlayer(100);

        this.GetComponentInParent<AIAnimationEvents>().AEWeaponColliderOff();//make sure collider is off as attack can be interrupted

        if (stateManager.GetComponent<AIHealth>().currentHealth <= 0)
        {
            state.SwitchToTheNextState(state.DeathState);
        }
    }

    public override void UpdateState(AIStateManager state)
    {
        CheckingAnimationFinsihed(state);
    }

    public override void ExitState(AIStateManager state)
    {
        controller.agent.isStopped = false;
    }  
    
    void CheckingAnimationFinsihed(AIStateManager state)
    {
        if (controller.IsAnimationDone(controller.anim, AIController.AnimState.Stagger))
        {
            state.SwitchToTheNextState(state.IdleState);
        }
    }
    
}
