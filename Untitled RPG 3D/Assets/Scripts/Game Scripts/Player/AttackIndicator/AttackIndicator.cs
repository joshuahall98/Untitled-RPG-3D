using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicator : MonoBehaviour
{
    Vector3 position;
    public GameObject attackIndicatorCanvas;
    public GameObject attackIndicator;
    public GameObject player;

    private void Start()
    {
        attackIndicatorCanvas = GameObject.Find("AttackIndicator");
        attackIndicator = GameObject.Find("AttackIndicatorImage");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }



        Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        //prevent mouse pointing up
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, 0);
        attackIndicatorCanvas.transform.rotation = Quaternion.Lerp(transRot, attackIndicatorCanvas.transform.rotation, 0f);
        //attackIndicatorCanvas.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);


    }
}
