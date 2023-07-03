using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStaggerState : AIState
{
    public AIIdle idleState;

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
            return idleState;
        }
        else
        {
            Debug.Log("Stagger");
            return this;
        }
        
    }

    public void AEStaggerFin()
    {
        Debug.Log("stagger false");
        isStaggered = false;
    }
    
}
