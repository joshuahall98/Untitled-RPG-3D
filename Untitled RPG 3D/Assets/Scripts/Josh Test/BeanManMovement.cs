using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeanManMovement : MonoBehaviour
{
    public PlayerInputActions playerInput;
    private CharacterController controller;

    public bool isMoving;

    Vector2 currentMoveInput;
    Vector3 actualMovement;
    Vector3 isometric;

    [SerializeField] private float speed = 8;

    InputAction move;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        move = playerInput.Player.Move;
    }

    // Update is called once per frame
    void Update()
    {
        currentMoveInput = move.ReadValue<Vector2>();
        actualMovement = new Vector3();
        //Condensed movement -- Converted y to z axis
        actualMovement.x = currentMoveInput.x;
        actualMovement.z = currentMoveInput.y;

        //magic code that converts the basic player movement into isometric
        isometric = new Vector3();
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        isometric = matrix.MultiplyPoint3x4(actualMovement);

        //move the character controller
        controller.Move(isometric * speed * Time.deltaTime);
        isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

        //Character Rotation
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(isometric.x, 0, isometric.z);
        Vector3 posLookAt = currentPos + newPos;
        transform.LookAt(posLookAt);

        controller.SimpleMove(Vector3.forward * 0);
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
}
