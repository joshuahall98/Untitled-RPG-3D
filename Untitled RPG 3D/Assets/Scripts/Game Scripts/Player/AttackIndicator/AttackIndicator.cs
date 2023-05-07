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
    Vector3 position;
    public GameObject attackIndicatorCanvas;
    public GameObject attackIndicator;
    public GameObject player;
    public LayerMask ground;

    float distanceFromCameraToGround;

    Vector3 p1;
    Vector3 p2;

    float distanceBetweenP1P2;
    float length;

    //have this accessed for all directional attacks
    [SerializeField]public Vector3 pointHerePlease;

    private void Start()
    {
        attackIndicatorCanvas = GameObject.Find("AttackIndicator");
        attackIndicator = GameObject.Find("AttackIndicatorImage");
        player = GameObject.Find("Player");
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

        //get the point of the mouse and record position
        //DONT KNOW IF THIS IS STILL NECCESARY?
        /*RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,ground))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }*/


        //rotate indicator based on player and mouse position
        Quaternion transRot = Quaternion.LookRotation(pointHerePlease - new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        //prevent mouse pointing up
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
        attackIndicatorCanvas.transform.rotation = Quaternion.Slerp(transRot, attackIndicatorCanvas.transform.rotation, 0f);
        //attackIndicatorCanvas.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
    }

    void PositionOfMouse()
    {

        //move indicator to mouse position
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ground))
        {
            //DISTANCE BETWEEN PLAYER HEIGHT AND RAYCAST ON GROUND
            p1 = new Vector3(hit.point.x, player.transform.position.y -0.8f, hit.point.z);
            p2 = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            distanceBetweenP1P2 = Vector3.Distance(p1, p2);

            //POINT THE INDICATOR ON THE GROUND
            //attackIndicator.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            attackIndicator.transform.position = pointHerePlease;

            //https://www.reddit.com/r/Unity3D/comments/p4ckq4/isometric_view_problem_that_i_dont_know_how_to/
            //https://www.mathsisfun.com/algebra/trig-finding-side-right-triangle.html
            //https://www.nuffieldfoundation.org/sites/default/files/files/FSMA%20Pythagoras%20theorem%20student.pdf

            //GET ANGLE OF ISOMETRIC CAMERA
            var deg = 60;
            //CONVERT THE DEGREE TO RADIAN
            var rad = deg * Mathf.Deg2Rad;
            //PASS RADIAN THROUGH TAN AND MUTIPLY BY DISTANCE BETWEEN PLAYER HIEGHT AND MOUSE POINT
            length = Mathf.Tan(rad) * distanceBetweenP1P2;
            //ALGORITHM FOR HYPOTONUSE
            float hypoto = Mathf.Pow(length, 2) + Mathf.Pow(distanceBetweenP1P2, 2);
            hypoto = Mathf.Sqrt(hypoto);
 
            //CALCULATE DISTANCE FROM CAMERA TO GROUND REMOVING HYPTONUSE
            distanceFromCameraToGround = hit.distance;
            distanceFromCameraToGround = (distanceFromCameraToGround - hypoto);

            //WHERE THE INDICATOR SHOULD POINT
            pointHerePlease = castPoint.GetPoint(distanceFromCameraToGround);

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

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
       
    }
}
