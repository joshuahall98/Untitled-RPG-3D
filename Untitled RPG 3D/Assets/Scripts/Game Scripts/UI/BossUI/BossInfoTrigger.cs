using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua

public class BossInfoTrigger : MonoBehaviour
{
    public string bossName;

    GameObject playerUI;

    public GameObject boss;


    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
        boss.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        playerUI.GetComponent<BossInfoUI>().ActivateBossStats(bossName);
        boss.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        DestroyTrigger();
    }

    public void DestroyTrigger()
    {
        Destroy(gameObject);
    }
}
