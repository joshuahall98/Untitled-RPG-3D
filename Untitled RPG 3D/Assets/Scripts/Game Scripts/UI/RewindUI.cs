using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Joshua

public class RewindUI : MonoBehaviour
{
    //UI
    public Image[] rewindFill;
    public Image[] rewindContainer;
    //these two ints are used to figure out which container you are currently trying to fill up
    int j = 0;
    public int k = 0;
    float random;
    public Image currentContainer;
    public Image currentFill;
    public Image refillingContainer;
    public HealthBar healthBar;
    public HealthBar rewindFadedHPBar;

    //action checkers
    public int rewindsLeft;
    public int maxRewinds;
    public float rewindFillXPAmount;
    public int rewindFadedHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rewindsLeft = PlayerRewind.rewindsLeft;
        maxRewinds = PlayerRewind.maxRewinds;
        rewindFillXPAmount = PlayerRewind.rewindFillXPAmount;
        rewindFadedHealth = PlayerRewind.rewindFadedHealth;



        //this fills up the rewind containers
        for (int i = 0; i < rewindFill.Length; i++)
        {
            if (i < rewindsLeft)
            {
                currentFill = rewindFill[i];
                currentFill.fillAmount = 1;
            }

        }

        // this makes sure the max rewind UI matches max rewinds
        for (int i = 0; i < rewindContainer.Length; i++)
        {
            if (i < maxRewinds)
            {
                currentContainer = rewindContainer[i];
                currentContainer.enabled = true;
            }
            else
            {
                currentContainer = rewindContainer[i];
                currentFill = rewindFill[i];
                currentFill.enabled = false;
                currentContainer.enabled = false;

            }

        }

        //filling up the hour glass
        if (rewindsLeft < maxRewinds)
        {
            refillingContainer = rewindFill[k];
            refillingContainer.fillAmount = rewindFillXPAmount;
        }

        //when you fill up a rewind container start the count from scratch
        if (rewindFillXPAmount >= 1)
        {
            PlayerRewind.rewindFillXPAmount = 0;
            PlayerRewind.rewindsLeft++;
            k = rewindsLeft + 1;

        }

    }

    //UI changes when you rewind
    public void Rewind()
    {

        k = PlayerRewind.rewindsLeft - 1;
        currentFill = rewindFill[k];
        currentFill.fillAmount = 0;
        if (rewindsLeft < rewindFill.Length)
        {
            currentFill = rewindFill[k + 1];
            currentFill.fillAmount = 0;
        }
        if (rewindsLeft == maxRewinds)
        {
            rewindFillXPAmount = 0;
        }

        
    }

    public void HealthBar()
    {
        healthBar.SetHealth(PlayerHealth.currentHP);
    }

    public void FadedHealthBar()
    {
        rewindFadedHPBar.SetHealth(rewindFadedHealth);
    }

}
