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

    private void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  
    }

    public void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            this.GetComponent<AIStateManager>().state = AIStateEnum.DEATH;
            this.GetComponent<Animator>().SetBool("isDead", true);
            this.GetComponent<CapsuleCollider>().enabled = false;

            //Play death anim
            //GetComponent<ConXP>().DropXP();

        }
    }
}
