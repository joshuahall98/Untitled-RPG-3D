using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine 
{
    public AiState[] states;
    public AIAgent agent;

    public AIStateID currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AiState[numStates];
    }

    public void RegisterState(AiState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AiState GetState(AIStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
