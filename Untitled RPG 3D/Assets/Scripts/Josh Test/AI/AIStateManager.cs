using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;


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

    [SerializeField]AIController controller;

    public bool angry; // to control whether AI is angry
    bool delayDone = false;

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
        if(currentState != StaggerState)
        {
            currentState.ExitState(this);
            controller.anim.Rebind();
            controller.RepeatAnimationState(AIController.AnimState.Stagger, 0, 0);//this has to be called here for looping animation
        //    SoundManager.SoundManagerInstance.SelectAudioClass("Wurgle");
            SoundManager.SoundManagerInstance.PlayOneShotSound("Test");
            currentState = StaggerState;
            currentState.EnterState(this);
        }
    }

}
