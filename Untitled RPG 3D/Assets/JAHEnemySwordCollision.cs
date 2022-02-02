using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHEnemySwordCollision : MonoBehaviour
{
    bool isPlayerDead;
    private void OnTriggerEnter(Collider collision)
    {

        if (JAHEnemySword.isAttacking == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
                
            } 
        }

}
}
 