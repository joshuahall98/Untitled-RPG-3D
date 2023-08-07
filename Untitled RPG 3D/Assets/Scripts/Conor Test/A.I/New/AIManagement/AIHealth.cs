using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Conor 

public class AIHealth : MonoBehaviour
{   
  
    public float maxHealth = 100;
    public float currentHealth;

    bool canTakeDamage;

    // public Slider healthBar;
    [SerializeField]
    AIAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
    }

    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage == true)
        {
            currentHealth -= damageAmount;
            canTakeDamage = false;
            StartCoroutine(Reset());
        }
          
    }

    //should stop enemy taking damage twice on the same frame for collisions
    private IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();

        canTakeDamage = true;
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

    /*public void CheckHealthPercentage()
    {
        if (currentHealth < (maxHealth / 2) && this.GetComponent<AIStateManager>().state != AIStateEnum.DEATH) 
        {
            this.GetComponent<AIStateManager>().state = AIStateEnum.FLEE;
        }
    }*/
}
