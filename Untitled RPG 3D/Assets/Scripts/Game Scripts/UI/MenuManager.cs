using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //AS OF RIGHT NOW THIS PAUSES THE GAME BUT NOT ALL THE INPUTS, MAKE SURE TO SET UP A MENU INPUT SYSTEM

    public PlayerInputActions playerInput;

    public bool paused = false;

    GameObject player;
    public GameObject menuUI;

    private void Awake()
    {
        playerInput = new PlayerInputActions();

        player = GameObject.Find("Player");
        menuUI = GameObject.Find("PlayerUI");

        playerInput.Menu.Pause.performed += PressEscMenu;
    }

    private void PressEscMenu(InputAction.CallbackContext press)
    {
        PressEsc();
    }

    public void PressEsc()
    {

        if (paused == false)
        {
            PauseGame();
            paused = true;
            player.GetComponent<PlayerController>().DisablePlayerActionMap();
            EnableMenuActionMap();
            menuUI.GetComponent<MenuUI>().EnablePauseText();
        }
        else
        {
            UnPauseGame();
            paused = false;
            player.GetComponent<PlayerController>().EnablePlayerActionMap();
            EnableMenuActionMap();
            menuUI.GetComponent<MenuUI>().DisablePauseText();
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

    public void EnableMenuActionMap()
    {
        playerInput.Menu.Enable();
    }

    public void DisableMenuActionMap()
    {
        playerInput.Menu.Disable();
    }

    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

}
