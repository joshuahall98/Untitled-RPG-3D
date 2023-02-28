using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

public class EnemyWeaponCollision : MonoBehaviour
{
    [SerializeField]float force;

    public EnemyScriptableObject Enemy;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            var dir = collision.transform.position - this.transform.position;
            var enemyPos = this.transform.position;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(Enemy.damage);
            collision.gameObject.transform.GetComponent<PlayerKnockback>().AddImpact(dir, enemyPos, force);
            //disable collider after hitting player so they can only be hit once
            GetComponent<Collider>().enabled = false;

        }

    }

}
