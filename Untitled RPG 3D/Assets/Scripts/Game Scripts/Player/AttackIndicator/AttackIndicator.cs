 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class AttackIndicator : MonoBehaviour
{
    Vector3 position;
    public GameObject attackIndicatorCanvas;
    public GameObject attackIndicator;
    public GameObject player;
    public LayerMask ground;

    private void Start()
    {
        attackIndicatorCanvas = GameObject.Find("AttackIndicator");
        attackIndicator = GameObject.Find("AttackIndicatorImage");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        PointFromPlayer();

        PositionOfMouse();

        Cursor.visible = false;

    }

    void PointFromPlayer()
    {

        //get the point of the mouse and record position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,ground))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }


        //rotate indicator based on player and mouse position
        Quaternion transRot = Quaternion.LookRotation(new Vector3(position.x, player.transform.position.y, position.z) - new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
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
            attackIndicatorCanvas.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        }

        //attackIndicator.transform.position = Camera.main.ScreenToWorldPoint(mouse);

    }
}
