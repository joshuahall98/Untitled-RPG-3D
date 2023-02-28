using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Conor 

public class AIHealth : MonoBehaviour
{
    public float maxHealth;

    // public Slider healthBar;
    [SerializeField]
    AIAgent agent;


    // Start is called before the first frame update
    void Start()
    {
       maxHealth =  agent.config.maxHP;

      //  healthBar.maxValue = maxHealth;
      //  healthBar.value = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
       // healthBar.transform.rotation = Camera.main.transform.rotation;
    }

    public void TakeDamage(float damageAmount)
    {
        maxHealth -= damageAmount;
        if (maxHealth <= 0)
        {
            maxHealth = 0;
            Destroy(gameObject);

            //Play death anim
            GetComponent<ConXP>().DropXP();

        }

       // healthBar.value = maxHealth;
       // healthBar.gameObject.SetActive(true);
    }
}
