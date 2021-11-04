using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHPlayerMovement : MonoBehaviour
{
    [SerializeField]Vector3 playerMoveInput;

    private CharacterController controller;
    private PlayerControls controls;


    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();

        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        controls.PlayerKeyboard.Movement.performed += playerMovement =>
        {
            playerMoveInput = new Vector3(playerMovement.ReadValue<Vector2>().x, playerMoveInput.y, playerMovement.ReadValue<Vector2>().y);
        };
controls.PlayerKeyboard.Movement.canceled += playerMovement =>
        {
            playerMoveInput = new Vector3(playerMovement.ReadValue<Vector2>().x, playerMoveInput.y, playerMovement.ReadValue<Vector2>().y);
        };
        

        Move();
    }

    private void Move()
    {
        Vector3 MoveVec = transform.TransformDirection(playerMoveInput);

        controller.Move(MoveVec * speed * Time.deltaTime);

        controller.SimpleMove(Vector3.forward * 0);
        
    }
}
