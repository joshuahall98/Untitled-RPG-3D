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
        controller.anim.SetBool("isChasing", false);//this has to be called after otherwise animator bugs
        controller.agent.isStopped = false;
        attackFin = false;
    }

    //call this when an the attack is finished so the AI can enter the next state
    public void AttackFin()
    {
        attackFin = true;
    }
    
}
