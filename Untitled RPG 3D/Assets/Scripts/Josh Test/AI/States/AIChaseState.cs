using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    public AIAttackState attackState;
    public AIIdle idleState;

    NavMeshAgent navMeshAgent;

    GameObject player;

    Animator anim;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponentInParent<Animator>();
        player = GameObject.Find("Player");
    }

    public override AIState RunCurrentState()
    {
        if (AIStateManager.state == AIStateEnum.ATTACK)//attack state
        {
            anim.SetBool("isChasing", false);
            return attackState;
        }
        else if (AIStateManager.state == AIStateEnum.IDLE)//idle state  THIS SHOULD ALSO GO TO PATROL WHERE APPLICABLE OR STAND STILL
        {
            anim.SetBool("isChasing", false);
            OutOfRange();
            return idleState;
        }
        else//chase state
        {
            ChasePlayer();
            ReturnToIdle();
            return this;
        }
    }

    void ReturnToIdle()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > 20)
        {
            AIStateManager.state = AIStateEnum.IDLE;
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.isStopped = false;
        anim.SetBool("isChasing", true);
        navMeshAgent.speed = 4;
        this.navMeshAgent.SetDestination(player.transform.position);
        if (Vector3.Distance(this.transform.position, player.transform.position) < 2)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            AIStateManager.state = AIStateEnum.ATTACK;
        }
    }

    void OutOfRange()
    {
        anim.SetBool("isChasing", false);
        this.navMeshAgent.SetDestination(this.transform.position);
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

}
  
