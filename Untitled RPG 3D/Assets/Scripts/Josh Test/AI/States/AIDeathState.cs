using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(AIStateManager state)
    {
        throw new System.NotImplementedException();
    }

    /*public override AIState RunCurrentState()
    {
        controller.GetComponent<Animator>().SetBool("isDead", true);
        controller.GetComponent<CapsuleCollider>().enabled = false;
        return this;
    }*/

    public override void UpdateState(AIStateManager state)
    {
        throw new System.NotImplementedException();
    }
}
