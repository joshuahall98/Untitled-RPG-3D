using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    bool attackFin = false;

    public override void EnterState(AIStateManager state)
    {

        controller.anim.SetTrigger("Attack");
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
        stateManager.state = AIStateEnum.ATTACK;
    }

    public override void UpdateState(AIStateManager state)
    {
        if(attackFin ==  true)
        {
            state.SwitchToTheNextState(state.IdleState);
        }
    }

    public override void ExitState(AIStateManager state)
    {
        controller.anim.SetBool("isChasing", false);//this has to be called after otherwise it bugs
        controller.agent.isStopped = false;
        attackFin = false;
    }

    public void AttackFin()
    {
        attackFin = true;
        Debug.Log("attack fone");
    }

    /*public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.IDLE)//idle state
        {
            
            return stateManager.idleState;
        }
        else
        {
            
            return this;
        }
        
    }*/

    
}
