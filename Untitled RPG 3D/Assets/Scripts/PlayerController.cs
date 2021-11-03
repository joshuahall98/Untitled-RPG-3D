using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    public PlayerInputActions playerInput;

    private Rigidbody rb;

    [SerializeField]
    private float speed = 5f;
 

    private void FixedUpdate()
    {
        Vector3 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        transform.position += moveInput * speed * Time.deltaTime;

    }
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerInput = new PlayerInputActions();
        

        playerInput.Player.Attack.performed += attackPerformed => lightAtk();
        playerInput.Player.Move.performed += movementPerformed => Movement(movementPerformed.ReadValue<Vector2>());
        playerInput.Player.Dash.performed += jumpPerformed => Dash();

    }


    void lightAtk()
    {
        Debug.Log("Light Attack");
    }

    void Movement(Vector2 direction)
    {

      //  Vector3 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        //transform.position += moveInput * speed * Time.deltaTime;
        Debug.Log("DisabledPerson" + direction);
        
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
