using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHPlayerPunch : MonoBehaviour
{

    public static bool isAttacking = false;

    private void OnCollisionEnter(Collision collision)
    {

        if (isAttacking == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(100);
            }
        }

    }

}
