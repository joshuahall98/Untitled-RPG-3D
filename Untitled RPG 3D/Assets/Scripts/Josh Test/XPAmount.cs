using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPAmount : MonoBehaviour
{
    private void OnTriggerEnter(Collider collide)
    {
        if (collide.tag == "Player")
        {

            collide.gameObject.GetComponent<PlayerRewind>().GetXPAmount();

        }

    }
}
