using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Conor 

public class AIHealth : MonoBehaviour
{

    [SerializeField]AIController controller;

    public float currentHealth;
    bool canTakeDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = controller.stats.maxHP;
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

    //MOVE THIS TO THE END OF THE PLAYER ATTACK ANIM
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
