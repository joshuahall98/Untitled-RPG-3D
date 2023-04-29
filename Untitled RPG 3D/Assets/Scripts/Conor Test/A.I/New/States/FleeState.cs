using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : AiState
{
    
    public AIStateID GetID()
    {
        return AIStateID.Flee;
    }
    public void Enter(AIAgent agent)
    {
       
    }

    public void Update(AIAgent agent)
    {
        agent.animator.SetBool("isFleeing", true);

        //Opposite direction to Player
        Vector3 playerDirection =  agent.transform.position - agent.Player.position;

        if (!agent.enabled)
        {
            return;
        }
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.transform.position + playerDirection;
        }
    }
    public void Exit(AIAgent agent)
    {

    }

}
