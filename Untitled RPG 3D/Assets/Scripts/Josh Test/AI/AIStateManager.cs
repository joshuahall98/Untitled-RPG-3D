using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum { IDLE, ATTACK, CHASE, FLEE, ROAM, STAGGER}
public class AIStateManager : MonoBehaviour
{
     [SerializeField]AIState currentState;
     public AIStateEnum state;

    private void Start()
    {
        state = AIStateEnum.IDLE;
    }

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

   /* private void NextStateEnum(AIStateEnum state)
    {
        
    }*/

    //Animation events
    public void AEStaggerFin()
    {
        this.GetComponentInChildren<AIStaggerState>().StaggerFin();
    }

    public void AEAttackFin()
    {
        this.GetComponentInChildren<AIAttackState>().AttackFin();
    }
}
