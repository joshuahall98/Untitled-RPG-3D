using FullscreenEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //AS OF RIGHT NOW THIS PAUSES THE GAME BUT NOT ALL THE INPUTS, MAKE SURE TO SET UP A MENU INPUT SYSTEM

    public PlayerInputActions playerInput;

    GameObject player;
    GameObject menuUI;

    GameObject gameManager;

    private void Awake()
    {
        playerInput = new PlayerInputActions();

        player = GameObject.Find("Player");
        menuUI = GameObject.Find("PlayerUI");
        gameManager = GameObject.Find("GameManager");

        playerInput.Menu.Pause.performed += PressPause;

    }

    private void Start()
    {
        DisableMenuActionMap();
    }

    private void PressPause(InputAction.CallbackContext press)
    {
        gameManager.GetComponent<GameManager>().PauseAndUnpause();
        MenuUIPauseUnpause();
    }

    public void MenuUIPauseUnpause()
    {

        if (GameManager.gameState == GameState.PAUSE)
        {
            EnableMenuActionMap();
            player.GetComponent<PlayerController>().DisablePlayerActionMap();
            menuUI.GetComponent<MenuUI>().EnablePauseText();
            Cursor.lockState = CursorLockMode.None;
        }
        else if(GameManager.gameState == GameState.PLAY)
        {
            DisableMenuActionMap();
            player.GetComponent<PlayerController>().EnablePlayerActionMap();
            menuUI.GetComponent<MenuUI>().DisablePauseText();
            Cursor.lockState = CursorLockMode.Confined;
            player.GetComponent<PlayerHeavyAttack>().HeavyAtkRelease();//stop heavy attack charge bug
        }
  
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
