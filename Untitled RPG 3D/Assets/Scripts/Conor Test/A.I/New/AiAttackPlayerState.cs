using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    
    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }
    public void Enter(AIAgent agent)
    {
        
    }
    public void Update(AIAgent agent)
    {

        //Attack Player
        Debug.Log("attack");
        agent.aiAttack.Attack();

        agent.stateMachine.ChangeState(AIStateID.ChasePlayer);

    }

    public void Exit(AIAgent agent)
    {
        agent.animator.SetBool("isAttacking", false);
    }

   

 
}
