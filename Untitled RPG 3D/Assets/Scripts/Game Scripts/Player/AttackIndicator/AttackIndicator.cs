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
    public GameObject player; // using player position for now, may use another object as the base of players position, we see.
    public LayerMask playerLayer;

    //have this accessed for all directional attacks
    [SerializeField]public Vector3 pointHerePlease;

    [SerializeField]GameObject testcube;

    private void Start()
    {
        attackIndicatorCanvas = GameObject.Find("AttackIndicator");
        attackIndicator = GameObject.Find("AttackIndicatorImage");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PointFromPlayer();
        Cursor.visible = false;
    }

    private void Update()
    {
        PositionOfMouse();
    }

    void PointFromPlayer()
    {

        //rotate indicator based on player and mouse position
        Quaternion transRot = Quaternion.LookRotation(pointHerePlease - new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        //prevent mouse pointing up
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
        attackIndicatorCanvas.transform.rotation = Quaternion.Lerp(transRot, attackIndicatorCanvas.transform.rotation, 0f);
        //attackIndicatorCanvas.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
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
            //https://www.nuffieldfoundation.org/sites/default/files/files/FSMA%20Pythagoras%20theorem%20student.pdf

            //GET ANGLE OF ISOMETRIC CAMERA
            var deg = 60;
            //CONVERT THE DEGREE TO RADIAN
            var rad = deg * Mathf.Deg2Rad;
            //PASS RADIAN THROUGH TAN AND MUTIPLY BY DISTANCE BETWEEN PLAYER HIEGHT AND MOUSE POINT
            float length = Mathf.Tan(rad) * distanceBetweenPlayerHeightAndRayHitpoint;
            //ALGORITHM FOR HYPOTONUSE
            float hypoto = Mathf.Pow(length, 2) + Mathf.Pow(distanceBetweenPlayerHeightAndRayHitpoint, 2);
            hypoto = Mathf.Sqrt(hypoto);

            //CALCULATE DISTANCE FROM CAMERA TO GROUND REMOVING HYPTONUSE
            float distanceFromCameraToGround = hit.distance;
            distanceFromCameraToGround = (distanceFromCameraToGround - hypoto);
            
            //DEPENDING ON PLAYER HEIGHT DEPENDS ON CAST
            if (player.transform.position.y > hit.point.y)
            {
                //PULL THE POINTER TOWARDS CAMERA IF PLAYER IS ABOVE HIT POINT
                pointHerePlease = castPoint.GetPoint(distanceFromCameraToGround);
    
            }
            else if(player.transform.position.y < hit.point.y)
            {
                //PUSH THE POINTER AWAY FROM CAMERA IF PLAYER IS BELOW HIT POINT
                pointHerePlease = castPoint.GetPoint(distanceFromCameraToGround + (hypoto * 2));
            }
            else if(player.transform.position.y == hit.point.y)
            {
                //DONT MOVE POINTER IF ON SAME LEVEL
                pointHerePlease = rayHitPoint;
            }

            //POSITION OF ATTACK INDICATOR
            attackIndicator.transform.position = pointHerePlease;

            testcube.transform.position = rayHitPoint;

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
