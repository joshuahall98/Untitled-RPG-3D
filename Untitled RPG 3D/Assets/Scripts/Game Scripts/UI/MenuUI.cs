using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI paused;
    public TextMeshProUGUI death;

    public void Awake()
    {
        DisablePauseText();
        DisableDeathText();
    }

    public void EnablePauseText()
    {
        paused.enabled = true;
    }

    public void DisablePauseText()
    {
        paused.enabled = false;
    }

    public void EnableDeathText()
    {
        death.enabled = true;
    }

    public void DisableDeathText()
    {
        death.enabled = false;
    }
}
