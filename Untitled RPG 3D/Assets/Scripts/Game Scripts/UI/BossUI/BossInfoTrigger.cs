using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoTrigger : MonoBehaviour
{
    public string bossName;

    GameObject playerUI;


    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
    }

    private void OnTriggerEnter(Collider other)
    {
        playerUI.GetComponent<BossInfoUI>().ActivateBossStats(bossName);
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
