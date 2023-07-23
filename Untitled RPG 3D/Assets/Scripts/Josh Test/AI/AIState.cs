using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    Animator animator;
    
    public abstract AIState RunCurrentState();

    
}