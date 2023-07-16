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

    public void AEWeaponColliderOn()
    {
        collider.enabled = true;
    }

    public void AEWeaponColliderOff()
    {
        collider.enabled = false;
    }
}
