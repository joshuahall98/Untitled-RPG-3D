using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStaggerState : AIState
{
    [SerializeField]AIStateManager stateManager;

    [SerializeField]bool isStaggered = true;

    Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }


    public override AIState RunCurrentState()
    {
        if(!isStaggered) 
        {
            isStaggered = true;
            return stateManager.idleState;
        }
        else
        {
            Debug.Log("Stagger");
            return this;
        }
        
    }

    public void StaggerFin()
    {
        Debug.Log("stagger false");
        isStaggered = false;
    }
    
}
