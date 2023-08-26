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

    [SerializeField]bool staggerFin = false;

    public override void EnterState(AIStateManager state)
    {

        staggerFin = false;
        controller.agent.isStopped = true;

        controller.RotateToPlayer();

        if (stateManager.GetComponent<AIHealth>().currentHealth <= 0)
        {
            state.SwitchToTheNextState(state.DeathState);
        }
    }

    public override void UpdateState(AIStateManager state)
    {
/*
        if (staggerFin == true)
        {
            state.SwitchToTheNextState(state.IdleState);
        }*/

        if (controller.IsAnimationDone(controller.anim, AIController.AnimState.Stagger))
        {
            //controller.ChangeAnimationState(AIController.AnimState.Idle, 0f, 0);//so we can transition back to stagger
            state.SwitchToTheNextState(state.IdleState);
        }

        //  controller.anim.ResetTrigger("Attack");//to stop animator transitioning to wrong animation
    }

    public override void ExitState(AIStateManager state)
    {
        staggerFin = false;
        controller.agent.isStopped = false;
  
    }


    // called at end of animation so AI can transition to next state
    public void StaggerFin()
    {
        staggerFin = true;
    }

    
    
}
