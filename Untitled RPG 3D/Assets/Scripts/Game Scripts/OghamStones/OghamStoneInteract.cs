using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OghamStoneInteract : MonoBehaviour
{
    public GameObject inspectionObj;
    GameObject ui;
    GameObject player;
    GameObject mouse;
    PlayerInputActions playerInput;

    bool inRange;
    bool isInteracting;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        playerInput.Player.Interact.performed += Interact;

    }

    private void Update()
    {
        // this is needed to prevent character from moving if you pause during inspect
        if(isInteracting == true)
        {
            player.transform.gameObject.GetComponent<PlayerController>().DisableHeavyAttackCharge();
            player.transform.gameObject.GetComponent<PlayerController>().DisableLightAttack();
            player.transform.gameObject.GetComponent<PlayerController>().DisableMovement();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRewind();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRoll();
        }
    }

    private void Start()
    {
        inspectionObj.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.Find("PlayerUI");
        mouse = GameObject.Find("AttackIndicator");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
            ui.GetComponent<InteractText>().InteractTextActive();
 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            ui.GetComponent<InteractText>().InteractTextInactive();
        }
    }

    void Interact(InputAction.CallbackContext InteractInput)
    {
        if(inRange == true)
        {
            if (inspectionObj.activeSelf)
            {
                inspectionObj.SetActive(false);
                ui.SetActive(true);
                mouse.SetActive(true);
                Cursor.visible = false;
                isInteracting = false;
                player.transform.gameObject.GetComponent<PlayerController>().EnableHeavyAttackCharge();
                player.transform.gameObject.GetComponent<PlayerController>().EnableLightAttack();
                player.transform.gameObject.GetComponent<PlayerController>().EnableMovement();
                player.transform.gameObject.GetComponent<PlayerController>().EnableRewind();
                player.transform.gameObject.GetComponent<PlayerController>().EnableRoll();
            }
            else
            {
                inspectionObj.SetActive(true);
                ui.SetActive(false);
                mouse.SetActive(false);
                Cursor.visible = true;
                isInteracting = true;
                
            }
        }
        
    }

    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }
}
