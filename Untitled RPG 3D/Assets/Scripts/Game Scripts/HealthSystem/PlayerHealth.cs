using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Animation
    Animator anim;

    public static bool isDead = false;
    public bool damageTaken = false;
    public float damageTakenTimer;
    public static int maxHP = 100;
    public static int currentHP = 0;
    public int currentHPVisible = 0;

    //UI
    public GameObject rewindUI;
    GameObject menuUI;


    //action checker
    public bool isRolling;
    public bool isRewinding;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        menuUI = GameObject.Find("PlayerUI");

        currentHP = maxHP;

        rewindUI = GameObject.Find("PlayerUI");
        rewindUI.GetComponent<PlayerHPBar>().SetMaxHP();
    }

    // Update is called once per frame
    void Update()
    {
        isRolling = PlayerController.isRolling;
        isRewinding = PlayerController.isRewinding;

        currentHPVisible = currentHP;

        //this used to prevent the player from being hit more than once, moved to the attack collider on enemy so that mutiple enemies can hit the player at once

        /*if(damageTakenTimer > 0)
        {
            damageTakenTimer -= Time.deltaTime;
            damageTaken = false;
        }*/

    }

    public void InputTakeDamage()
    {
        TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        if (!isRewinding)
        {
            if (!isRolling /*&& damageTaken == false*/)
            {
                currentHP -= damage;

                /*damageTaken = true;
                damageTakenTimer = 1;*/

                rewindUI.GetComponent<PlayerHPBar>().AlterHP();
            }
        }
        
        

        if (currentHP == 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            PlayerController.isDead = true;
            menuUI.GetComponent<MenuUI>().EnableDeathText();
        }
    }

}
