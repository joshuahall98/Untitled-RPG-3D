using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    [SerializeField]AIStateManager stateManager;

    NavMeshAgent navMeshAgent;

    GameObject player;

    [SerializeField] Animator anim;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.ATTACK)//attack state
        {
            anim.SetBool("isChasing", false);
            return stateManager.attackState;
        }
        else if (stateManager.state == AIStateEnum.IDLE)//idle state  
        {
            anim.SetBool("isChasing", false);
            OutOfRange();
            return stateManager.idleState;
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
            stateManager.state = AIStateEnum.IDLE;
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
            stateManager.state = AIStateEnum.ATTACK;
        }
    }

    void OutOfRange()
    {
        anim.SetBool("isChasing", false);
        this.navMeshAgent.SetDestination(this.transform.position);
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

}
  
