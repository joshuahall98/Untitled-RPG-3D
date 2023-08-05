using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;
    [SerializeField] float dist;

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.ATTACK)//attack state
        {
            return stateManager.attackState;
        }
        else if (stateManager.state == AIStateEnum.IDLE)//idle state  
        {
            OutOfRange();
            return stateManager.idleState;
        }
        /*else if (stateManager.state == AIStateEnum.STAGGER)//stagger state
        {
            StopMovement();
            controller.anim.SetBool("isChasing", false);
            return stateManager.staggerState;
        }*/
        else//chase state
        {
            dist = Vector3.Distance(this.transform.position, player.transform.position);

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
        controller.agent.isStopped = false;
        controller.anim.SetBool("isChasing", true);
        controller.agent.speed = 4;
        this.controller.agent.SetDestination(player.transform.position);
        if (Vector3.Distance(this.transform.position, player.transform.position) < 1.5f)
        {
            
            controller.anim.SetTrigger("Attack");
            StopMovement();
            stateManager.state = AIStateEnum.ATTACK;
        }
    }

    void OutOfRange()
    {
        controller.anim.SetBool("isChasing", false);
        this.controller.agent.SetDestination(this.transform.position);
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

    void StopMovement()
    {
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
    }

    
}
  
