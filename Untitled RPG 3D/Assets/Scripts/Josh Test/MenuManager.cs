using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //AS OF RIGHT NOW THIS PAUSES THE GAME BUT NOT ALL THE INPUTS, MAKE SURE TO SET UP A MENU INPUT SYSTEM

    public bool paused = false;

    public void PressEsc()
    {

        if (paused == false)
        {
            PauseGame();
            paused = true;
            
        }
        else
        {
            UnPauseGame();
            paused = false;
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void UnPauseGame()
    {
        Time.timeScale = 1;
    }

}
