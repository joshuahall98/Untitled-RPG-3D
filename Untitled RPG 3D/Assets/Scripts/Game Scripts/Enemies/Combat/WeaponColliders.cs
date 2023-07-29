using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

public class WeaponColliders : MonoBehaviour
{
    Collider collider;

    [SerializeField]GameObject weapon;

    private void Start()
    {
        collider = weapon.GetComponent<Collider>();
        collider.enabled = false;
    }

    public void WeaponColliderOn()
    {
        collider.enabled = true;
    }

    public void WeaponColliderOff()
    {
        collider.enabled = false;
    }
}
