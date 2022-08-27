using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDashStats : MonoBehaviour
{
    public WeaponScriptableObject Weapon;

    float dashSpeed;
    float dashDistance;

    public void WeaponDashSpeed()
    {
        dashSpeed = Weapon.dashSpeed;
        AttackDash.speed = dashSpeed;
    }

    public void WeaponDashDistance()
    {
        dashDistance = Weapon.dashDistance;
        AttackDash.dashDistance = dashDistance;
    }
}
