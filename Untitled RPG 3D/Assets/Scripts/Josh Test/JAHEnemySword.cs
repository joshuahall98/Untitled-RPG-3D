using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHEnemySword : MonoBehaviour
{
    public static bool isAttacking = false;

    private void Update()
    {
        if (isAttacking == true)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (isAttacking == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
            }
        }
    }

}
