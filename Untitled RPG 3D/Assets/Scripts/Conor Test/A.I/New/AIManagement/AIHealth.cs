using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Conor 

public class AIHealth : MonoBehaviour
{   
  
    public float maxHealth = 100;
    public float currentHealth;

    // public Slider healthBar;
    [SerializeField]
    AIAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  
    }

    /*public void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            this.GetComponent<AIStateManager>().IsDead();
            //GetComponent<ConXP>().DropXP();

        }
    }*/

    public void CheckHealthPercentage()
    {
        if (currentHealth < (maxHealth / 2) && this.GetComponent<AIStateManager>().state != AIStateEnum.DEATH) 
        {
            this.GetComponent<AIStateManager>().state = AIStateEnum.FLEE;
        }
    }
}
