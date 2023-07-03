using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHideState : AIState
{
    public override AIState RunCurrentState()
    {
        //AI is hiding
        return this;
    }

    
}
