using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHEnemySwordCollision : MonoBehaviour
{
    bool isPlayerDead;

    public float force;

    private void OnTriggerEnter(Collider collision)
    {

        if (JAHEnemySword.isAttacking == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                var dir = collision.transform.position - this.transform.position;
                var enemyPos = this.transform.position;
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(5);
                collision.gameObject.transform.GetComponent<PlayerKnockback>().AddImpact(dir, enemyPos, 200);
                
            } 
        }

}
}
 