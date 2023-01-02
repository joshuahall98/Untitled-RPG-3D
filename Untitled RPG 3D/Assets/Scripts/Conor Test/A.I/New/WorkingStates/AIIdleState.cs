using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AiState
{
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }
    public void Enter(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        Vector3 playerDirection = agent.Player.position - agent.transform.position;
        if(playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if(dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }
    public void Exit(AIAgent agent)
    {
       
    }

   

}
