using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStaggerState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override AIState RunCurrentState()
    {
        if(stateManager.state == AIStateEnum.IDLE) 
        {
            controller.anim.ResetTrigger("isHit");
            return stateManager.idleState;
        }
        else
        {
            controller.anim.SetTrigger("isHit");
            return this;
        }
        
    }

    public void StaggerFin()
    {
        stateManager.state = AIStateEnum.IDLE;
    }
    
}
