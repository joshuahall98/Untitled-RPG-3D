using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    //THIS SCRIPT CHOOSES THE DAMAGE A WEAPON CAN DO WHEN IT COLLIDES WITH AN ENEMY

    public static bool isAttacking = false;

    [SerializeField] int damage = 100;

    private void OnCollisionEnter(Collision collision)
    {

        if (isAttacking == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }

    }
}
