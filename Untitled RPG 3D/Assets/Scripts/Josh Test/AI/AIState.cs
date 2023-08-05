using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{

    public abstract void EnterState(AIStateManager state);

    public abstract void UpdateState(AIStateManager state);

    public abstract void ExitState(AIStateManager state);


    //public abstract AIState RunCurrentState();

    
}
