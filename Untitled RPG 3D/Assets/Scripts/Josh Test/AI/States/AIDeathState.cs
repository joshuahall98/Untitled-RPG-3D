using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override AIState RunCurrentState()
    {
        Debug.Log("I am dead");
        return this;
    }

}
