using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponCollision : MonoBehaviour
{
    public float force;

    bool playerTakingDamage;

    public EnemyScriptableObject Enemy;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player" /*&& playerTakingDamage == false*/)
        {
            var dir = collision.transform.position - this.transform.position;
            var enemyPos = this.transform.position;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(Enemy.damage);
            collision.gameObject.transform.GetComponent<PlayerKnockback>().AddImpact(dir, enemyPos, force);
            GetComponent<Collider>().enabled = false;
            //playerTakingDamage = true;

        }

    }

    //this is the old system for preventing an enemy collider hitting you more than once
    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(PlayerDamageDelay());
        }
    }

    IEnumerator PlayerDamageDelay()
    {
        yield return new WaitForSeconds(0.5f);

        playerTakingDamage = false;
    }*/
}
