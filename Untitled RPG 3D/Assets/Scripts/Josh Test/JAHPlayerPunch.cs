using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHPlayerPunch : MonoBehaviour
{

    public static bool attacking = false;
    public bool currentAttackVisible = false;

    private void OnCollisionEnter(Collision collision)
    {

        if (attacking == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(20);
            }
        }

    }

    private void Update()
    {
        currentAttackVisible = attacking;
    }
}
