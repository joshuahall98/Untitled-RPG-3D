using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{

    public PlayerController PlayerObject;
    public int dmg;

   

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.tag == "Player")
        {
            Destroy(gameObject);
            PlayerObject.rewindsLeft++;
                    
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("PlayerCollided");
            PlayerObject.PlayerTakeDamage(dmg);
            Destroy(gameObject);
        }
    }



}
