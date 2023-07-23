using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.IDLE)//idle state
        {
            controller.agent.isStopped = false;
            return stateManager.idleState;
        }
        else if (stateManager.state == AIStateEnum.STAGGER)//stagger state
        {
            controller.agent.isStopped = false;
            controller.anim.SetTrigger("isHit");
            controller.agent.velocity = Vector3.zero;
            controller.agent.isStopped = true;
            return stateManager.staggerState;
        }
        else
        {
            return this;
        }
        
    }

    public void AttackFin()
    {
        stateManager.state = AIStateEnum.IDLE;
    }
}
