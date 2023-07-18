using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    //THIS SCRIPT CHOOSES THE DAMAGE A WEAPON CAN DO WHEN IT COLLIDES WITH AN ENEMY

    public static bool isAttacking = false;

    public WeaponScriptableObject Weapon;

    [SerializeField]float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerController.state == PlayerState.ATTACKING)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit");
                other.gameObject.GetComponentInParent<AIHealth>().TakeDamage(damage);
                other.gameObject.GetComponentInParent<AIStateManager>().IsHit();
            }
        }
    }

    public void LightAttackDamage()
    {
        damage = Weapon.baseDmg;
    }

    public void HeavyAttackDamage()
    {
        damage = Weapon.heavyDmg;
    }


}
