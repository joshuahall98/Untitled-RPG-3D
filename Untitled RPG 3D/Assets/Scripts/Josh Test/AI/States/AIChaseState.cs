using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    NavMeshAgent navMeshAgent;

    GameObject player;



    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.ATTACK)//attack state
        {
            controller.anim.SetBool("isChasing", false);
            return stateManager.attackState;
        }
        else if (stateManager.state == AIStateEnum.IDLE)//idle state  
        {
            controller.anim.SetBool("isChasing", false);
            OutOfRange();
            return stateManager.idleState;
        }
        else if (stateManager.state == AIStateEnum.STAGGER)//stagger state
        {
            StopMovement();
            controller.anim.SetBool("isChasing", false);
            return stateManager.staggerState;
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
        controller.anim.SetBool("isChasing", true);
        navMeshAgent.speed = 4;
        this.navMeshAgent.SetDestination(player.transform.position);
        if (Vector3.Distance(this.transform.position, player.transform.position) < 2)
        {
            StopMovement();
            stateManager.state = AIStateEnum.ATTACK;
        }
    }

    void OutOfRange()
    {
        controller.anim.SetBool("isChasing", false);
        this.navMeshAgent.SetDestination(this.transform.position);
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

    void StopMovement()
    {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
    }

}
  
