using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Conor 

public enum AIStateID
{
    //Add New States in here, this will be the TAG name.
    Idle,
    ChasePlayer,
    Attack,
    Death,
    Flee,
    Patrol,
    Celebration
}
public interface AiState
{

    AIStateID GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
