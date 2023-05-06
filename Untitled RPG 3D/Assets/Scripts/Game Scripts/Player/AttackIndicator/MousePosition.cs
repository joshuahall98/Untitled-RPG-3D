using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    //THIS SCRIPT IS FOR THE CUSTOM MOUSE POSITION WHICH WE USE TO SHOW THE DIRECTION YOU CAN SWING YOUR SWORD, IT IS PLACED ONTO AN OBJECT SO THE OBJECT CAN ACT AS THE INDICATOR
    public Transform customMouse;

    public LayerMask ground;

    public bool isHit;

    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = false;

        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            customMouse.transform.position = new Vector3(hit.point.x, hit.point.y + 2, hit.point.z);

            //LAYER MASK IGNORES COLLIDERS, USE COLLIDERS WHEN YOU WANT TO STOP A RAYCAST
            /*if(hit.collider.tag == "Ground")
            {
                customMouse.transform.position = hit.point;
                isHit = true;
            }
            else
            {
                isHit = false;
            }*/
        }

    }
}
