using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    Renderer invisibleObj;
    void Start()
    {
        invisibleObj = GetComponent<MeshRenderer>();
        invisibleObj.enabled = false;
    }


}
