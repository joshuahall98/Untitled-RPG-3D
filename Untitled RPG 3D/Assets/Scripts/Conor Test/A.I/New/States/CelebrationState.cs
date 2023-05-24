using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CelebrationState : AiState
{
    public AIStateID GetID()
    {
        return AIStateID.Celebration;
    }
    public void Enter(AIAgent agent)
    {
        Debug.Log("Winners!");
    }

    public void Update(AIAgent agent)
    {
       agent.navMeshAgent.velocity = Vector3.zero;
       
       //Play Celebration anim
    }
    public void Exit(AIAgent agent)
    {

    }

    

}

