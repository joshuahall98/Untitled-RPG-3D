using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField]AIIdle idleState;

    NavMeshAgent navMeshAgent;

    Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    public override AIState RunCurrentState()
    {
        if (AIStateManager.state == AIStateEnum.IDLE)//idle state
        {
            anim.ResetTrigger("Attack");
            navMeshAgent.isStopped = false;
            return idleState;
        }
        else
        {
            anim.SetTrigger("Attack");
            return this;
        }
        
    }

    public void AttackFin()
    {
        AIStateManager.state = AIStateEnum.IDLE;
    }
}
