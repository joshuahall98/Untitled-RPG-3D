using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum { IDLE, ATTACK, CHASE, FLEE, ROAM, STAGGER}
public class AIStateManager : MonoBehaviour
{
    //states scripts
    public AIState currentState;
    public AIIdle idleState;
    public AIChaseState chaseState;
    public AIAttackState attackState;
    public AIFleeState fleeState;
    public AIStaggerState staggerState;
    public AIHideState hideState;

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

    public void Hit()
    {
        state = AIStateEnum.IDLE;
        state = AIStateEnum.STAGGER;
    }


}
