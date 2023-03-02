using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    //THIS SCRIPT CHOOSES THE DAMAGE A WEAPON CAN DO WHEN IT COLLIDES WITH AN ENEMY

    public static bool isAttacking = false;

    public WeaponScriptableObject Weapon;

    [SerializeField] int damage = 100;

    /*private void OnCollisionEnter(Collision collision)
    {

        if (isAttacking == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }

    }*/

    public void OnTriggerEnter(Collider collision)
    {
        if (PlayerController.state == PlayerState.ATTACKING)
        {
            if (collision.gameObject.tag == "Enemy")
           {
                collision.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
               
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
