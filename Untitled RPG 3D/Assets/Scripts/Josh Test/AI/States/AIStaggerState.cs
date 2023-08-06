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

   /* public override AIState RunCurrentState()
    {
        if(stateManager.state == AIStateEnum.IDLE) 
        {
            
            return stateManager.idleState;
        }
        else
        {
            return this;
        }
        
    }*/

    public void StaggerFin()
    {

        //stateManager.state = AIStateEnum.IDLE;
        staggerFin = true;
        Debug.Log("stagger done");

        
    }
    
}
