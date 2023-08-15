using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
        controller.GetComponent<Animator>().SetBool("isDead", true);
        controller.agent.velocity = Vector3.zero;
        controller.agent.isStopped = true;
        controller.GetComponent<CapsuleCollider>().enabled = false;

        controller.RotateToPlayer();

    }

    public override void UpdateState(AIStateManager state)
    {

    }

    public override void ExitState(AIStateManager state)
    {

    }

}
