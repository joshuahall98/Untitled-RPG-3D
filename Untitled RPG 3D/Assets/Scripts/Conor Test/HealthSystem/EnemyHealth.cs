using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;
    public static int currentHP = 0;
    public int currentHPVisible = 0;


    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
