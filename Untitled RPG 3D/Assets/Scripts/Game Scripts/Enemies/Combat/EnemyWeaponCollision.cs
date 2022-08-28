using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponCollision : MonoBehaviour
{
    public float force;

    bool playerTakingDamage;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player" && playerTakingDamage == false)
        {
            var dir = collision.transform.position - this.transform.position;
            var enemyPos = this.transform.position;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(5);
            collision.gameObject.transform.GetComponent<PlayerKnockback>().AddImpact(dir, enemyPos, force);
            playerTakingDamage = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerTakingDamage = false;
        }
    }
}
