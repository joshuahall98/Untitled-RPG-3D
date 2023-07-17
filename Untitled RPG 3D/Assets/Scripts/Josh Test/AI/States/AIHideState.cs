using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHideState : AIState
{
    [SerializeField]AIStateManager stateManager;

    public override AIState RunCurrentState()
    {
        //AI is hiding
        return this;
    }

    
}
