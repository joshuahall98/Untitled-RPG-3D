using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{

    private Quaternion LockPos;
    // Start is called before the first frame update
    void Awake()
    {
        LockPos = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = LockPos;   
    }
}
