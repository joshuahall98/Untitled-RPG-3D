using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSetup : MonoBehaviour
{

    public GameObject mouseIndicator;
    public GameObject playerUI;
    public GameObject gameManager;

    private void Start()
    {
        Instantiate(mouseIndicator);
        Instantiate(playerUI);
        Instantiate(gameManager);
    }
}
