using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

public class WeaponColliders : MonoBehaviour
{
    Collider collider;

    public GameObject weapon;

    private void Start()
    {
        collider = weapon.GetComponent<Collider>();
        collider.enabled = false;
    }

    public void AEcolliderOn()
    {
        collider.enabled = true;
    }

    public void AEcolliderOff()
    {
        collider.enabled = false;
    }
}
