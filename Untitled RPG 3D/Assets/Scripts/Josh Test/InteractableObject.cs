using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public bool inRange;

    [SerializeField]GameObject ui;
    [SerializeField]GameObject mouse;

    // Start is called before the first frame update
    void Start()
    {
        FindGameObjects();
    }

    public virtual void FindGameObjects()
    {
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
            other.GetComponent<PlayerController>().ObtainInteractableObject(this.gameObject);
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

    public virtual void PressInteract()
    { 

        if (PlayerController.state == PlayerState.INTERACTING && inRange == true)
        {
            ui.SetActive(false);
            mouse.SetActive(false);
            Cursor.visible = true;
        }
        else if (PlayerController.state != PlayerState.INTERACTING && inRange == true)
        {
            ui.SetActive(true);
            mouse.SetActive(true);
            Cursor.visible = false;
        }
    }

    
}
