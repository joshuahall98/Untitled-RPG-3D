using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStaggerState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override AIState RunCurrentState()
    {
        if(stateManager.state == AIStateEnum.IDLE) 
        {
            controller.agent.isStopped = false;
            return stateManager.idleState;
        }
        else
        {
            return this;
        }
        
    }

    public void StaggerFin()
    {
        stateManager.state = AIStateEnum.IDLE;
    }
    
}
