using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI paused;

    public void Awake()
    {
        DisablePauseText();
    }

    public void EnablePauseText()
    {
        paused.enabled = true;
    }

    public void DisablePauseText()
    {
        paused.enabled = false;
    }
}
