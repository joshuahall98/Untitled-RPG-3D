using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    GameObject playerUI;
    GameObject player;

    [TextArea]
    public string[] dialogue;
    string text;
    int i = 0;

    bool inRange;

    public PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        playerInput.Player.Interact.performed += Interact;

    }

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
        player = GameObject.Find("Player");
    }


    public void DialogueText()
    {
        if (i < dialogue.Length)
        {
            text = dialogue[i];
            i++;
            playerUI.GetComponent<DialogueUI>().DialogueText(text);
        }
        else
        {
            playerUI.GetComponent<DialogueUI>().EndDialogue();
            EndDialogue();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DialogueText();
            player.transform.gameObject.GetComponent<PlayerController>().DisableHeavyAttackCharge();
            player.transform.gameObject.GetComponent<PlayerController>().DisableLightAttack();
            player.transform.gameObject.GetComponent<PlayerController>().DisableMovement();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRewind();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRoll();
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    void Interact(InputAction.CallbackContext InteractInput)
    {
        if (inRange == true)
        {
            DialogueText();
        }


    }

    void EndDialogue()
    {
        player.transform.gameObject.GetComponent<PlayerController>().EnableHeavyAttackCharge();
        player.transform.gameObject.GetComponent<PlayerController>().EnableLightAttack();
        player.transform.gameObject.GetComponent<PlayerController>().EnableMovement();
        player.transform.gameObject.GetComponent<PlayerController>().EnableRewind();
        player.transform.gameObject.GetComponent<PlayerController>().EnableRoll();
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
