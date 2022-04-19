using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHXPAmount : MonoBehaviour
{

    

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.tag == "Player")
        {

            collide.gameObject.GetComponent<PlayerRewind>().XPAmount();

        }

    }
}
