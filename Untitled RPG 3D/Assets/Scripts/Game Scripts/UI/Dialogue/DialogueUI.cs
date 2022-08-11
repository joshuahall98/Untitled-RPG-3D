using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{

    public GameObject dialogueBox;

    public TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);   
    }

    public void DialogueText(string text)
    {
        if(dialogueBox.activeSelf == false)
        {
            dialogueBox.SetActive(true);
        }
        dialogueText.text = text;
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}
