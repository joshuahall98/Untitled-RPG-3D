using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

public class EnemyWeaponCollision : MonoBehaviour
{
    [SerializeField] AIController controller;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            var dir = collision.transform.position - this.transform.position;
            var enemyPos = this.transform.position;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(controller.stats.damage);
            collision.gameObject.transform.GetComponent<PlayerKnockback>().AddImpact(dir, enemyPos, controller.stats.knockBackForce);   
            GetComponent<Collider>().enabled = false; //disable collider after hitting player so they can only be hit once

        }

    }

}
