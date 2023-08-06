using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum { IDLE, ATTACK, CHASE, FLEE, ROAM, STAGGER, DEATH, HIDE}
public class AIStateManager : MonoBehaviour
{
    //states scripts
    public AIState currentState;
    public AIIdle IdleState;
    public AIChaseState ChaseState;
    public AIAttackState AttackState;
    public AIStaggerState StaggerState;
    public AIDeathState DeathState;
    public AIFleeState FleeState;
    public AIHideState HideState;

    public bool angry; // to control whether AI is angry
    bool delayDone = false;
    
    
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
        StartCoroutine(DelayedStart());
    }

    //this delay stops the error when instantiating AI, the problem is with the start method calling late
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);

        currentState = IdleState;

        currentState.EnterState(this);

        delayDone = true;
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

        if(delayDone == true)
        {
            currentState.UpdateState(this);
        }
        
    }
    
    public void SwitchToTheNextState( AIState nextState )
    {
        currentState.ExitState(this);
        currentState = nextState;
        currentState.EnterState(this);
    }

    public void IsHit()
    {
        state = AIStateEnum.STAGGER;
        if(currentState != StaggerState)
        {
            currentState.ExitState(this);
            currentState = StaggerState;
            currentState.EnterState(this);
        }
    }

}
