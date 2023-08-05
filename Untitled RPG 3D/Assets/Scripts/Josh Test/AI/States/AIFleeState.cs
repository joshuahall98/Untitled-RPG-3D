using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFleeState : AIState
{
    bool hidden;

    [SerializeField]AIStateManager stateManager;

    GameObject player;

    NavMeshAgent navMeshAgent;

    private void Start()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponentInParent<NavMeshAgent>();

    }

    public override AIState RunCurrentState()
    {
        if (hidden)
        {
            //enemy hides
            return stateManager.hideState;
        }
        else 
        {
            Vector3 playerDirection = this.transform.position - player.transform.position;

            this.navMeshAgent.destination = this.transform.position + playerDirection;

            return this;
        }
       
    }

    
}
