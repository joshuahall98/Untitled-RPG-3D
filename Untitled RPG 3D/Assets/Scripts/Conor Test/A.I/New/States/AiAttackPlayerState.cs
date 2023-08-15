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
        agent.animator.SetTrigger("isAttacking");
        agent.navMeshAgent.isStopped = true;
     }
    public void Update(AIAgent agent)
    {

        //Attack Player
        Debug.Log("attack");     

        agent.stateMachine.ChangeState(AIStateID.ChasePlayer);

        //Less than 80%
         /*if(agent.aiHealth.currentHealth <= agent.aiHealth.maxHealth * 80/100){

                agent.stateMachine.ChangeState(AIStateID.Flee);
        }*/

    
    }

    public void Exit(AIAgent agent)
    {
        
    }

   

 
}
