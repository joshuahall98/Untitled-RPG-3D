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

    public override void EnterState(AIStateManager state)
    {
        //unused
    }

    public override void UpdateState(AIStateManager state)
    {
        //dist = Vector3.Distance(this.transform.position, player.transform.position);


        //return to idle
        if (Vector3.Distance(this.transform.position, player.transform.position) > 20)
        {
            stateManager.state = AIStateEnum.IDLE;
            controller.anim.SetBool("isChasing", false);
            state.SwitchToTheNextState(state.IdleState);
            stateManager.angry = false;
        }

        //chase player
        controller.agent.isStopped = false;
        controller.anim.SetBool("isChasing", true);
        controller.agent.speed = 4;
        this.controller.agent.SetDestination(player.transform.position);

        //attack state
        if (Vector3.Distance(this.transform.position, player.transform.position) < 1.5f)
        {
            state.SwitchToTheNextState(state.AttackState);
        }
    }

    public override void ExitState(AIStateManager state)
    {
        this.controller.agent.SetDestination(this.transform.position);
    }

    /*public override AIState RunCurrentState()
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
        else//chase state
        {
            

            ChasePlayer();
            ReturnToIdle();
            return this;
        }
    }*/

    void ReturnToIdle()
    {
        
        
    }

    void ChasePlayer()
    {
        
    }

    void OutOfRange()
    {
        
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

    void StopMovement()
    {
       
    }

    
}
  
