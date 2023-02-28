using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

//this script rotates world space UI hp bars

public class LookAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
