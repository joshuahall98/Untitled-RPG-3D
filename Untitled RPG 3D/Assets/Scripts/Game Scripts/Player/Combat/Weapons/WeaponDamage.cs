using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
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
