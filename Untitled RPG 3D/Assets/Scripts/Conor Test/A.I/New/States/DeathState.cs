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
        //Put Die in here
        Debug.Log("is dead");
    }

    public void Update(AIAgent agent)
    {
      
    }

    public void Exit(AIAgent agent)
    {
       
    }





 
}
