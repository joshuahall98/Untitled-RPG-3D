using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{

    public GameObject dialogueBox;
    public GameObject dialoguePotraits;

    public TextMeshProUGUI dialogueText;

    public GameObject playerPotrait;
    public GameObject playerPotraitFaded;

    public GameObject sidecharacterPotrait;
    public GameObject sidecharacterPotraitFaded;
    public GameObject wurlPotrait;
    public GameObject wurlPotraitFaded;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        dialoguePotraits.SetActive(false);
    }

    public void DialogueText(string text)
    {
        if(dialogueBox.activeSelf == false)
        {
            dialogueBox.SetActive(true);
            dialoguePotraits.SetActive(true);
        }
        dialogueText.text = text;
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        dialoguePotraits.SetActive(false);
    }

    //Potraits
    public void Portrait(string playerPortraitString, string otherPortraitsString)
    {
        //player
        if (playerPortraitString == PlayerPortrait.Player.ToString())
        {
            playerPotrait.SetActive(true);
            playerPotraitFaded.SetActive(false); 

        }
        else if (playerPortraitString == PlayerPortrait.PlayerFaded.ToString())
        {
            playerPotrait.SetActive(false);
            playerPotraitFaded.SetActive(true);

        }
        else if(playerPortraitString == PlayerPortrait.None.ToString())
        {
            playerPotrait.SetActive(false);
            playerPotraitFaded.SetActive(false);
        }


        //sidecharacter
        if (otherPortraitsString == OtherPotraits.SideCharacter.ToString())
        {
            sidecharacterPotrait.SetActive(true);
            sidecharacterPotraitFaded.SetActive(false);
        }
        else if (otherPortraitsString == OtherPotraits.SideCharacterFaded.ToString())
        {
            sidecharacterPotrait.SetActive(false);
            sidecharacterPotraitFaded.SetActive(true);
        }
        else
        {
            sidecharacterPotrait.SetActive(false);
            sidecharacterPotraitFaded.SetActive(false);
        }

        //wurl
        if (otherPortraitsString == OtherPotraits.Wurl.ToString())
        {
            wurlPotrait.SetActive(true);
            wurlPotraitFaded.SetActive(false);
        }
        else if (otherPortraitsString == OtherPotraits.WurlFaded.ToString())
        {
            wurlPotrait.SetActive(false);
            wurlPotraitFaded.SetActive(true);
        }
        else
        {
            wurlPotrait.SetActive(false);
            wurlPotraitFaded.SetActive(false);
        }


    }

}
