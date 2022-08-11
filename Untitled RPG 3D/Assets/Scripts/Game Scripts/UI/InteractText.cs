using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractText : MonoBehaviour
{
    public TextMeshProUGUI interact;

    private void Start()
    {
        interact.enabled = false;
    }

    public void InteractTextActive()
    {
        interact.enabled = true;
    }

    public void InteractTextInactive()
    {
        interact.enabled = false;
    }
}
