using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{
    public AIState currentState;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState();

        if(nextState != null ) 
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState( AIState nextState )
    {
        currentState = nextState;
    }

    //Animation events
    public void AEStaggerFin()
    {
        this.GetComponentInChildren<AIStaggerState>().AEStaggerFin();
    }
}
