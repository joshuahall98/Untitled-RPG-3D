using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    int maxHP = 0;
    static int currentHP = 0;
    int currentHPVisible = 0;

    public EnemyScriptableObject Enemy;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = Enemy.maxHP;
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        currentHPVisible = currentHP;

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        

        healthBar.SetHealth(currentHP);

        if(currentHP == 0)
        {
           
            GetComponent<EnemyTest>().Death();
            GetComponent<ConXP>().DropXP();
        }
    }
}
