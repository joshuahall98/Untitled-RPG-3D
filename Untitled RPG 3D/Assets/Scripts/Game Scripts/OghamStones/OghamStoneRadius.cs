using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OghamStoneRadius : MonoBehaviour
{

    public GameObject swirls;

    public static bool inRange = false;

    private void Start()
    {
        swirls.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
            swirls.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            swirls.SetActive(false);
        }
    }

}
