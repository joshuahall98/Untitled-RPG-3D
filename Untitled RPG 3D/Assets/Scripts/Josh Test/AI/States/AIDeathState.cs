using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.GetComponent<Animator>().SetBool("isDead", true);
        controller.GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void UpdateState(AIStateManager state)
    {

    }

    public override void ExitState(AIStateManager state)
    {

    }

    /*public override AIState RunCurrentState()
    {
        
        return this;
    }*/

    
}
