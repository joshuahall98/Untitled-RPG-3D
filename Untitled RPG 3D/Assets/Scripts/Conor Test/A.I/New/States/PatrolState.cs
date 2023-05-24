using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : AiState
{
    public AIStateID GetID()
    {
        return AIStateID.Patrol;
    }
    public void Enter(AIAgent agent)
    {
        Debug.Log("Patrolling");
    }

    public void Update(AIAgent agent)
    {
        //Opposite direction to Player
        
        //Patrol
    }
    public void Exit(AIAgent agent)
    {

    }

    

}

