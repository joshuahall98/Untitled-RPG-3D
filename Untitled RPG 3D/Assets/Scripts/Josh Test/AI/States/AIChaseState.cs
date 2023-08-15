using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
        //unused
    }

    public override void UpdateState(AIStateManager state)
    {

        //return to idle
        if (Vector3.Distance(this.transform.position, controller.player.transform.position) > controller.stats.sightDistance * 2)
        {
            controller.anim.SetBool("isChasing", false);
            state.SwitchToTheNextState(state.IdleState);
            stateManager.angry = false;
        }

        //chase player
        controller.agent.isStopped = false;
        controller.anim.SetBool("isChasing", true);
        controller.agent.speed = controller.stats.speed;
        this.controller.agent.SetDestination(controller.player.transform.position);

        //attack state
        if (Vector3.Distance(this.transform.position, controller.player.transform.position) < controller.stats.attackRange)
        {
            state.SwitchToTheNextState(state.AttackState);
        }
    }

    public override void ExitState(AIStateManager state)
    {
        this.controller.agent.SetDestination(this.transform.position);
    }
    
}
  
