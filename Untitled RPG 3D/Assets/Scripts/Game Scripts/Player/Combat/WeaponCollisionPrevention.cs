using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionPrevention : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //prevent the rb from colliding with sword
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
