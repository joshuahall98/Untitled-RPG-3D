using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum { IDLE, ATTACK, CHASE, FLEE, ROAM, STAGGER, DEATH, HIDE}
public class AIStateManager : MonoBehaviour
{
    //states scripts
    [SerializeField]AIState currentState;
    public AIIdle IdleState;
    public AIChaseState ChaseState;
    public AIAttackState AttackState;
    public AIStaggerState StaggerState;
    public AIDeathState DeathState;
    public AIFleeState FleeState;
    public AIHideState HideState;
    
    
    /*public AIIdle idleState;
    public AIChaseState chaseState;
    public AIAttackState attackState;
    public AIFleeState fleeState;
    public AIStaggerState staggerState;
    public AIHideState hideState;
    public AIDeathState deathState;*/

    public AIStateEnum state;

    private void Start()
    {
        state = AIStateEnum.IDLE;

        currentState = IdleState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        /*RunStateMachine();
        if (state == AIStateEnum.STAGGER)
        {
            currentState = staggerState;
        }
        else if (state == AIStateEnum.DEATH)
        {
            currentState = deathState;
        }
        else if(state == AIStateEnum.FLEE)
        {
            currentState = fleeState;
        }*/


        currentState.UpdateState(this);
    }

   /* private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState();

        if(nextState != null ) 
        {
            SwitchToTheNextState(nextState);
        }
    }*/

    public void SwitchToTheNextState( AIState nextState )
    {
        currentState.ExitState(this);
        currentState = nextState;
        currentState.EnterState(this);
    }

    public void IsHit()
    {
        state = AIStateEnum.STAGGER;
    }


}
