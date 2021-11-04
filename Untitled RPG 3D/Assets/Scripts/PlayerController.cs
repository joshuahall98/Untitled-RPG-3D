using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] Vector3 playerMoveInput;
    [SerializeField] private float speed = 5f;

    public PlayerInputActions playerInput;

    private Rigidbody rb;

   

   




    private void FixedUpdate()
    {

        playerInput.Player.Move.performed += playerMovement =>
        {
            playerMoveInput = new Vector3(playerMovement.ReadValue<Vector2>().x, playerMoveInput.y, playerMovement.ReadValue<Vector2>().y);
        };

        Movement();

       

    }
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Attack.performed += attackPerformed => lightAtk();
        playerInput.Player.Move.performed += movementPerformed => Movement();
        playerInput.Player.Dash.performed += jumpPerformed => Dash();

    }


    void lightAtk()
    {
        Debug.Log("Light Attack");
    }

    void Movement()
    {

        Vector3 MoveVec = transform.TransformDirection(playerMoveInput);

        controller.Move(MoveVec * speed * Time.deltaTime);

        controller.SimpleMove(Vector3.forward * 0);
    }

    void Dash()
    {
        Debug.Log("RolliePollie");
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
