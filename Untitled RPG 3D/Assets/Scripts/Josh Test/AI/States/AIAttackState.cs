using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public override AIState RunCurrentState()
    {
        anim.SetTrigger("Attack");

        return this;
    }
}
