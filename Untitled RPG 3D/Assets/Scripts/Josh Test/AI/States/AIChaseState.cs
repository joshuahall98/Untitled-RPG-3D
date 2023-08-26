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
        controller.agent.isStopped = false;
        controller.ChangeAnimationState(AIController.AnimState.Chase, 0.1f, 0);
        //controller.anim.SetBool("isChasing", true);
        controller.agent.speed = controller.stats.speed;
    }

    public override void UpdateState(AIStateManager state)
    {

        //return to idle
        if (Vector3.Distance(this.transform.position, controller.player.transform.position) > controller.stats.deagroRange)
        {
            state.SwitchToTheNextState(state.IdleState);
            stateManager.angry = false;
        }

        //chase player
        this.controller.agent.SetDestination(controller.player.transform.position);

        //attack state
        if (Vector3.Distance(this.transform.position, controller.player.transform.position) < controller.stats.attackRange)
        {
            state.SwitchToTheNextState(state.AttackState);
        }
    }

    public override void ExitState(AIStateManager state)
    {
       // controller.anim.SetBool("isChasing", false);
        this.controller.agent.SetDestination(this.transform.position);
    }
    
}
  
