using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Animation
    Animator anim;

    public static bool isDead = false;
    public int maxHP = 100;
    public static int currentHP = 0;
    public int currentHPVisible = 0;


    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        currentHPVisible = currentHP;

    }

    public void InputTakeDamage()
    {
        TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        healthBar.SetHealth(currentHP);

        if (currentHP == 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            //GetComponent<PlayerController>().DisableMovement();
            PlayerController.isDead = true;
            /*GetComponent<PlayerLightAttack>().DisableAttack();
            GetComponent<PlayerHeavyAttack>().DisableHeavyAttackCharge();*/
        }
    }

}
