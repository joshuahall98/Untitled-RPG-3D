using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    //THIS SCRIPT IS FOR THE UI AND CONTROLS THE PLAYERS HP

    public HealthBar healthBar;

    public void SetMaxHP()
    {
        healthBar.SetHealth(PlayerHealth.maxHP);
    }

    public void AlterHP()
    {
        healthBar.SetHealth(PlayerHealth.currentHP);
    }
}
