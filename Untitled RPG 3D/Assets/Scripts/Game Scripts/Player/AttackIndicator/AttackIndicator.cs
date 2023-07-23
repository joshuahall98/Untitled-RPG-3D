 using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

//THIS SCRIPT CONTROLS THE POINTER AND ALLOWS THE PLAYER TO AIM ATTACKS
public class AttackIndicator : MonoBehaviour
{
    public GameObject attackIndicatorCanvas;
    public GameObject attackIndicator;
    public GameObject player; // this looks better when attached to object at players feet
    public LayerMask playerLayer;

    //have this accessed for all directional attacks
    Vector3 pointHerePlease;

    /*[SerializeField] GameObject testcube;
    [SerializeField] GameObject testcube2;
    [SerializeField] GameObject testcube3;*/

    private void Awake()
    {
        attackIndicatorCanvas = GameObject.Find("AttackIndicator");
        attackIndicator = GameObject.Find("AttackIndicatorImage");
        
    }

    private void Start()
    {
        Cursor.visible = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        PointFromPlayer();
        
    }

    private void Update()
    {
        PositionOfMouse();
    }

    void PointFromPlayer()
    {
        if(attackIndicator != null)
        {
            //rotate indicator based on player and mouse position
            Quaternion transRot = Quaternion.LookRotation(pointHerePlease - new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
            //prevent mouse pointing up
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            attackIndicatorCanvas.transform.rotation = Quaternion.Lerp(transRot, attackIndicatorCanvas.transform.rotation, 0f);
            //attackIndicatorCanvas.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
        }

    }

    void PositionOfMouse()
    {

        //move indicator to mouse position
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        //cast to every layer but the player layer 
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ~playerLayer))
        {
            //DISTANCE BETWEEN PLAYER HEIGHT AND RAYCAST ON GROUND
            Vector3 playerHeight = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
            Vector3 rayHitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            float distanceBetweenPlayerHeightAndRayHitpoint = Vector3.Distance(playerHeight, rayHitPoint);

            //https://www.mathsisfun.com/algebra/trig-finding-side-right-triangle.html

            //GET ANGLE OF ISOMETRIC CAMERA
            var deg = 30;

            //CONVERT THE DEGREE TO RADIAN
            var rad = deg * Mathf.Deg2Rad;

            //PASS RADIAN THROUGH SIN AND DIVIDE DISTANCE BETWEEN PLAYER HIEGHT AND MOUSE POINT BY THE RESULT TO OBTAIN HYPOTENUSE 
            float hypote = distanceBetweenPlayerHeightAndRayHitpoint / (Mathf.Sin(rad));

            //CALCULATE DISTANCE FROM CAMERA TO GROUND REMOVING HYPTONUSE
            float distanceFromCameraToGround = hit.distance;
            
            //DEPENDING ON PLAYER HEIGHT DEPENDS ON CAST
            if (player.transform.position.y > hit.point.y)
            {
                //PULL THE POINTER TOWARDS CAMERA IF PLAYER IS ABOVE HIT POINT
                pointHerePlease = castPoint.GetPoint(distanceFromCameraToGround - hypote);
    
            }
            else if(player.transform.position.y < hit.point.y)
            {
                //PUSH THE POINTER AWAY FROM CAMERA IF PLAYER IS BELOW HIT POINT
                pointHerePlease = castPoint.GetPoint(distanceFromCameraToGround + hypote);
            }
            else if(player.transform.position.y == hit.point.y)
            {
                //DONT MOVE POINTER IF ON SAME LEVEL
                pointHerePlease = rayHitPoint;
            }

            //POSITION OF ATTACK INDICATOR
            attackIndicator.transform.position = pointHerePlease;

            /*testcube.transform.position = rayHitPoint;
            testcube2.transform.position = playerHeight;
            testcube3.transform.position = pointHerePlease;*/

        }
    }

    //All attacks and spells call this function to aim
    public void Aim()
    {
        //confirm mouse is being used
        if(PlayerController.lastDevice.displayName == "Mouse")
        {
            // Calculate the direction
            var direction = pointHerePlease - transform.position;

            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
       
    }
}
