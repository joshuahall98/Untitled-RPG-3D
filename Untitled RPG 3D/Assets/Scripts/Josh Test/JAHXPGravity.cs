using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHXPGravity : MonoBehaviour
{

    Rigidbody rb;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            rb.useGravity = false;
        }
    }
  
}
