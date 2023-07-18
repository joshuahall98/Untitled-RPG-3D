using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.IDLE)//idle state
        {
            controller.anim.ResetTrigger("Attack");
            navMeshAgent.isStopped = false;
            return stateManager.idleState;
        }
        else if (stateManager.state == AIStateEnum.STAGGER)//stagger state
        {
            controller.anim.ResetTrigger("Attack");
            navMeshAgent.isStopped = false;
            return stateManager.staggerState;
        }
        else
        {
            controller.anim.SetTrigger("Attack");
            return this;
        }
        
    }

    public void AttackFin()
    {
        stateManager.state = AIStateEnum.IDLE;
    }
}
