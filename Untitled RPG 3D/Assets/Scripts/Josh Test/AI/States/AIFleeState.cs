using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;
using UnityEngine.AI;

public class AIFleeState : AIState
{

    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");

    }

    public override void EnterState(AIStateManager state)
    {
        controller.anim.SetBool("isFleeing", true);
        controller.anim.ResetTrigger("Hit");
    }

    public override void UpdateState(AIStateManager state)
    {
        Vector3 playerDirection = this.transform.position - player.transform.position;

        controller.agent.destination = this.transform.position + playerDirection;

        //return to idle
        if (Vector3.Distance(this.transform.position, player.transform.position) > 20)
        {
            controller.anim.SetBool("isFleeing", false);
            state.SwitchToTheNextState(state.HideState);
            stateManager.angry = false;
        }
    }

    public override void ExitState(AIStateManager state)
    {

    }
}
