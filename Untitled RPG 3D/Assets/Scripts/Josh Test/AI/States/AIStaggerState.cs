using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStaggerState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    [SerializeField]bool staggerFin = false;

    public override void EnterState(AIStateManager state)
    {
        staggerFin = false;
        controller.agent.isStopped = true;
        controller.anim.SetTrigger("Hit");

        if (stateManager.GetComponent<AIHealth>().currentHealth <= 0)
        {
            state.SwitchToTheNextState(state.DeathState);
        }
    }

    public override void UpdateState(AIStateManager state)
    {

        if (staggerFin == true)
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        controller.anim.ResetTrigger("Attack");//to stop animator transitioning to wrong animation
    }

    public override void ExitState(AIStateManager state)
    {
        staggerFin = false;
        controller.agent.isStopped = false;
  
    }

    public void StaggerFin()
    {
        staggerFin = true;
    }
    
}
