using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Josh
public class PlayerDash : MonoBehaviour
{
    //THIS SCRIPT CAUSES THE PLAYER TO DASH TO A LOCATION WHEN THEY USE A WEAPON, CAN ALSO BE USED TO DASH IN GENERAL

    [SerializeField]PlayerScriptableObject stats;

    private CharacterController controller;

    Vector3 dashDirection;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    //called on animation start
    public void DashActionAnimStart()
    {
        StartCoroutine(DashAction());
    }

    //called by anim event on attack
    private IEnumerator DashAction()
    {
        dashDirection = transform.rotation * Vector3.forward;

        float startTime = Time.time;

        while (Time.time < startTime + stats.dashTime)
        {
            controller.Move(dashDirection * stats.dashSpeed * Time.deltaTime);
            yield return null;

        }
    }
}
