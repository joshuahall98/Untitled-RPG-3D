using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class OghamStoneInteract : MonoBehaviour
{
    public GameObject inspectionObj;
    GameObject ui;
    GameObject mouse;

    bool inRange;

    private void Update()
    {
        if(PlayerController.state == PlayerState.INTERACTING  && inRange == true)
        {
            inspectionObj.SetActive(true);
            ui.SetActive(false);
            mouse.SetActive(false);
            Cursor.visible = true;
        }
        else if (PlayerController.state != PlayerState.INTERACTING && inRange == true)
        {
            inspectionObj.SetActive(false);
            ui.SetActive(true);
            mouse.SetActive(true);
            Cursor.visible = false;
        }
    }

    private void Start()
    {
        inspectionObj.SetActive(false);
        ui = GameObject.Find("PlayerUI");
        mouse = GameObject.Find("AttackIndicator");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.inRange = true; 
            inRange = true;
            ui.GetComponent<InteractText>().InteractTextActive();
 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            PlayerController.inRange = false;
            ui.GetComponent<InteractText>().InteractTextInactive();
        }
    }
 
}
