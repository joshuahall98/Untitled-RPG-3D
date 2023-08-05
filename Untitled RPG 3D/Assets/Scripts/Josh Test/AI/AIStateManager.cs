using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum { IDLE, ATTACK, CHASE, FLEE, ROAM, STAGGER, DEATH}
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
    public AIDeathState deathState;

    public AIStateEnum state;

    private void Start()
    {
        state = AIStateEnum.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
        if (state == AIStateEnum.STAGGER)
        {
            currentState = staggerState;
        }
        else if (state == AIStateEnum.DEATH)
        {
            currentState = deathState;
        }
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

    public void IsHit()
    {
        state = AIStateEnum.STAGGER;
    }


}
