using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AiState
{
    float timer = 0.0f;
    float distFromPlayer;


    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
  
    }

    public void Update(AIAgent agent)
    {

        //Track distance from Enemy to Player
        distFromPlayer = Vector3.Distance(agent.Player.transform.position, agent.navMeshAgent.transform.position);

        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.Player.position;
        }
        if (timer < 0.0f)
        {
            Vector3 direction = (agent.Player.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.Player.position;
                }
            }
            timer = agent.config.maxTime;
        }

        if (distFromPlayer <= agent.config.attackRadius)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
            
        }
        // If in attack radius switch to attack state

        
    }

    public void Exit(AIAgent agent)
    {
        
    }

  
   

 
}