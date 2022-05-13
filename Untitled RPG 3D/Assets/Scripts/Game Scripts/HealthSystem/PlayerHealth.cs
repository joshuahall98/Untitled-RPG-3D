using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Animation
    Animator anim;

    public static bool isDead = false;
    public static int maxHP = 100;
    public static int currentHP = 0;
    public int currentHPVisible = 0;

    //UI
    public GameObject rewindUI;
    GameObject menuUI;
    //public HealthBar healthBar;

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
        currentHPVisible = currentHP;

    }

    public void InputTakeDamage()
    {
        TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        rewindUI.GetComponent<PlayerHPBar>().AlterHP();

        if (currentHP == 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            PlayerController.isDead = true;
            menuUI.GetComponent<MenuUI>().EnableDeathText();
        }
    }

}
