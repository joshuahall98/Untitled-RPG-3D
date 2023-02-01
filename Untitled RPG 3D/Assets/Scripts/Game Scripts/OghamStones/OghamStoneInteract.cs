using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class OghamStoneInteract : InteractableObject
{
    public GameObject inspectionObj;

    private void Start()
    {
        inspectionObj.SetActive(false);
        FindGameObjects();
    }

    public override void FindGameObjects()
    {
        base.FindGameObjects();

    }

    public override void PressInteract()
    {
        base.PressInteract();

        if (PlayerController.state == PlayerState.INTERACTING && inRange == true)
        {
            inspectionObj.SetActive(true);

        }
        else if (PlayerController.state != PlayerState.INTERACTING && inRange == true)
        {
            inspectionObj.SetActive(false);

        }
    }

}
