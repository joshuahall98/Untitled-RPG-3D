using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterStats
{
    //Animation
    Animator anim;

    bool damageTaken = false;
    [SerializeField] float damageTakenTimer;
    public static int maxHP = 100;
    public static int currentHP = 0;
    int currentHPVisible = 0;

    //UI
    GameObject rewindUI;
    GameObject menuUI;

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

        //currentHPVisible = currentHP;

    }

    /*public void InputTakeDamage()
    {
        TakeDamage(20);
    }*/

    public void TakeDamage(int damage)
    {
        //everything but rewinding, rolling, dead
        if(PlayerController.state == PlayerState.IDLE || PlayerController.state == PlayerState.DIZZY || PlayerController.state == PlayerState.MOVING 
            || PlayerController.state == PlayerState.ATTACKING || PlayerController.state == PlayerState.INTERACTING || PlayerController.state == PlayerState.KNOCKEDDOWN)
        {
            if(PlayerController.immune == false) 
            {
                currentHP -= damage;

                rewindUI.GetComponent<PlayerHPBar>().AlterHP();

                currentHPVisible = currentHP;
            }
          
        }

        CheckIfDead();
    }

    public void CheckIfDead()
    {
        if (currentHP == 0)
        {
            anim.SetTrigger("Dead");
            menuUI.GetComponent<MenuUI>().EnableDeathText();

            PlayerController.state = PlayerState.DEAD;
        }
    }

    //remove the hud after player rewindsa from death
    public void DeathRewind()
    {
        menuUI.GetComponent<MenuUI>().DisableDeathText();
    }

}
