using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerPortrait { None, Player, PlayerFaded }
enum OtherPotraits { None, SideCharacter, SideCharacterFaded, Wurl, WurlFaded}

//Attach this script to trigger colliders to start a text sequence
public class DialogueTrigger : MonoBehaviour
{
    GameObject playerUI;
    GameObject player;

    [TextArea]
    public string[] dialogue;
    string text;
    string playerPortraitString;
    string otherPortraitsString;
    int i = 0;

    bool inRange;
    bool isInteracting;

    public PlayerInputActions playerInput;

    [SerializeField] OtherPotraits[] otherPotraits;
    [SerializeField] PlayerPortrait[] playerPortrait;

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

    private void Update()
    {
        if(isInteracting == true)
        {
            player.transform.gameObject.GetComponent<PlayerController>().DisableHeavyAttackCharge();
            player.transform.gameObject.GetComponent<PlayerController>().DisableLightAttack();
            player.transform.gameObject.GetComponent<PlayerController>().DisableMovement();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRewind();
            player.transform.gameObject.GetComponent<PlayerController>().DisableRoll();
        }
    }


    public void DialogueText()
    {
        isInteracting = true;
        inRange = true;
        if (i < dialogue.Length)
        {
            text = dialogue[i];
            ChosenUIPotrait();
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
        isInteracting = false;
        player.transform.gameObject.GetComponent<PlayerController>().EnableHeavyAttackCharge();
        player.transform.gameObject.GetComponent<PlayerController>().EnableLightAttack();
        player.transform.gameObject.GetComponent<PlayerController>().EnableMovement();
        player.transform.gameObject.GetComponent<PlayerController>().EnableRewind();
        player.transform.gameObject.GetComponent<PlayerController>().EnableRoll();
    }

    void ChosenUIPotrait()
    {
        playerPortraitString = playerPortrait[i].ToString();
        otherPortraitsString = otherPotraits[i].ToString();
        playerUI.GetComponent<DialogueUI>().Portrait(playerPortraitString, otherPortraitsString);

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
