using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackDash : MonoBehaviour
{
    //THIS SCRIPT CAUSES THE PLAYER TO DASH TO A LOCATION WHEN THEY USE A WEAPON, CAN ALSO BE USED TO DASH IN GENERAL

    private CharacterController controller;

    [SerializeField]float speed;
    [SerializeField]float dashTime = 0.5f;
    Vector3 dashDirection;

    public GameObject sword;


    void Awake()
    {

        controller = GetComponent<CharacterController>();

    }

    //called by anim event on attack
    public IEnumerator DashAction()
    {
        dashDirection = transform.rotation * Vector3.forward;

       // Debug.Log("attack");

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {

            controller.Move(dashDirection * speed * Time.deltaTime);
            yield return null;

        }
    }
}
