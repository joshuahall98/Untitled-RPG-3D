using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : AiState
{
    public AIStateID GetID()
    {
        return AIStateID.Death;
    }
    public void Enter(AIAgent agent)
    {
  
        Debug.Log("Dead");
        
       agent.navMeshAgent.velocity = Vector3.zero;
       
       //Play Animation 
    }

    public void Update(AIAgent agent)
    {
      
    }

    public void Exit(AIAgent agent)
    {
       
    }





 
}
