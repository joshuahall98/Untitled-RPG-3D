using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    Idle,
    ChasePlayer,
    Attack,
    Death
}
public interface AiState
{

    AIStateID GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
