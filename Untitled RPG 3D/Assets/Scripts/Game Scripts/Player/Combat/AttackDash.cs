using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDash : MonoBehaviour
{
    //THIS SCRIPT CAUSES THE PLAYER TO DASH TO A LOCATION WHEN THEY USE A WEAPON, CAN ALSO BE USED TO DASH IN GENERAL

    private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;

    public bool hitReg = false;
    public bool canDash = false;
    public static float dashDistance;
    public static float speed;

    Vector3 startingPoint;
    float distTravelled;

    public GameObject sword;

    Ray ray;
    RaycastHit hit;

    void Awake()
    {
        sword.GetComponent<WeaponDashStats>().WeaponDashDistance();
        sword.GetComponent<WeaponDashStats>().WeaponDashSpeed();

        mainCamera = Camera.main;

        controller = GetComponent<CharacterController>();

    }

    private void FixedUpdate()
    {
        if (hitReg == true)
        {

            //this decides max dash
            distTravelled = Vector3.Distance(transform.position, startingPoint);
            if (distTravelled >= dashDistance)
            {
                hit.point = transform.position;
            }

            //this prevents character from floating up when dashing
            if (hit.point.y >= transform.position.y)
            {
                var newPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                hit.point = newPosition;
            }

            //this move character to location
            if (canDash == true)
            {
                var offset = hit.point - transform.position;
                if (offset.magnitude > 0.1f)
                {
                    var offset2 = offset.normalized * speed;

                    controller.Move(offset2 * Time.deltaTime);

                    //need to call this so the character controller knows the rotation after you attack
                    GetComponent<PlayerController>().Rotation(transform.rotation);

                }
            }
            

            //this completes the action when players velocity goes below X amount
            if (controller.velocity.magnitude < 2f)
            {
                hitReg = false;

                
            }

        }
    }


    public void Dash()
    {

        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {

            startingPoint = transform.position;

            hitReg = true;
            canDash = true;

        }
    }
}
