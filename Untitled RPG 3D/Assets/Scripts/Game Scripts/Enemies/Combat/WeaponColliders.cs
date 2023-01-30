using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliders : MonoBehaviour
{
    Collider collider;

    public GameObject weapon;

    private void Start()
    {
        collider = weapon.GetComponent<Collider>();
        collider.enabled = false;
    }

    public void colliderOn()
    {
        collider.enabled = true;
        Debug.Log("On");
    }

    public void colliderOff()
    {
        collider.enabled = false;
        Debug.Log("off");
    }
}
