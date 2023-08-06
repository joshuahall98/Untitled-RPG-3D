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

    NavMeshAgent navMeshAgent;

    private void Start()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponentInParent<NavMeshAgent>();

    }

    /*public override AIState RunCurrentState()
    {
        if (stateManager.state == AIStateEnum.HIDE)//hide state
        {
            return stateManager.hideState;
        }
        else 
        {
            

           // 

            return this;
        }
       
    }*/

    public override void EnterState(AIStateManager state)
    {
        controller.anim.SetBool("isFleeing", true);
    }

    public override void UpdateState(AIStateManager state)
    {
        Vector3 playerDirection = this.transform.position - player.transform.position;

        this.navMeshAgent.destination = this.transform.position + playerDirection;
    }

    public override void ExitState(AIStateManager state)
    {

    }
}
