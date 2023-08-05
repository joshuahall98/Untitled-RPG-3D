using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHideState : AIState
{
    [SerializeField]AIStateManager stateManager;

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
        //AI is hiding
        return this;
    }*/

    public override void UpdateState(AIStateManager state)
    {
        throw new System.NotImplementedException();
    }
}
