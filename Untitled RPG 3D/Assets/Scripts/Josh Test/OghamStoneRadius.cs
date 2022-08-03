using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OghamStoneRadius : MonoBehaviour
{

    public static bool inRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }

}
