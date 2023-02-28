using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Joshua

public class SideCharacterController : MonoBehaviour
{
    //inputs and movement
    private CharacterController controller;
    [SerializeField] private float speed = 8;
    Vector2 currentMoveInput;
    Vector3 actualMovement;
    Vector3 isometric;
    public PlayerInputActions playerInput;

    //input actions
    InputAction move;

    //player
    GameObject player;

    //camera
    GameObject camera;

    //action checker
    //these action checkers are universal and other scripts must access them in the update to make sure they all correspond
    public static bool isMoving;

    private void Awake()
    {
        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.SwitchCharacters.performed += damagePerformed => SwitchCharacters();

        move = playerInput.Player.Move;

        player = GameObject.Find("Player");

        camera = GameObject.Find("Camera");

        DisableSideCharacterEnablePlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason
    }

    void SwitchCharacters()
    {
        camera.GetComponent<CameraControls>().SwitchCharacter();

        DisableSideCharacterEnablePlayer();
    }

    void DisableSideCharacterEnablePlayer()
    {
        /*player.GetComponent<AttackIndicator>().enabled = true;
        player.GetComponent<PlayerKnockback>().enabled = true;
        player.GetComponent<AttackDash>().enabled = true;
        player.GetComponent<PlayerHeavyAttack>().enabled = true;
        player.GetComponent<PlayerLightAttack>().enabled = true;
        player.GetComponent<CooldownSystem>().enabled = true;
        player.GetComponent<PlayerHealth>().enabled = true;
        player.GetComponent<AttackAim>().enabled = true;
        player.GetComponent<PlayerRewind>().enabled = true;*/
        player.GetComponent<PlayerController>().enabled = true;

        this.GetComponent<SideCharacterController>().enabled = false;
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
