using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoUI : MonoBehaviour
{

    public GameObject wurlTitle;
    public GameObject wurlHP;

    private void Start()
    {
        wurlHP.SetActive(false);
        wurlTitle.SetActive(false);
    }

    public void ActivateBossStats(string name)
    {
        if(name == "wurl")
        {
            wurlHP.SetActive(true);
            wurlTitle.SetActive(true);
        }
    }


}
