using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using ClipperLib;
using Unity.Mathematics;
using Random = UnityEngine.Random;
//using UnityEngine.iOS;
using System.Web.Mvc;
using UnityEngine.InputSystem.Utilities;

//Joshua

//THIS SCRIPT CONTROLS THE PLAYER STATES AND ALL PLAYER CONTROLS
public enum PlayerState { IDLE, MOVING, ROLLING, ATTACKING, DEAD, REWINDING, DIZZY, KNOCKEDDOWN, FALLING, INTERACTING}
public class PlayerController : MonoBehaviour
{
    #region - VARIABLES -
    //inputs and movement
    private CharacterController controller;
    [SerializeField] private float speed = 8;
    float baseSpeed;
    Vector2 currentMoveInput;
    Vector3 actualMovement;
    [SerializeField]Vector3 isometric;
    Vector3 posLookAt;
    public PlayerInputActions playerInput;
    [SerializeField]float gravity;
    [SerializeField]private LayerMask groundMask;

    /*public string hello;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;*/

    //used for turning camera, need to move this off the player controller
    [SerializeField]int isometricRotation = 45;

    //Animation
    Animator anim;

    //States
    public static PlayerState state;
    [SerializeField]PlayerState visibleState;

    //these bools are helpful for animations and state control
    [SerializeField]bool isMoving;
    bool isGrounded;
    bool isDizzy;

    //to check whether or not an object is within range for interaction
    public static bool inRange =  false;

    //to check if player character is immune
    public static bool immune = false;

    //Roll
    float rollCDTimer = 0;
    [SerializeField]int rollUsed = 0;
    [SerializeField]float rollSpeed = 2;
    [SerializeField]float rollTime = 0.5f;
    public GameObject dizzyAffect;
    Vector3 rollDirection;
    

    //input actions
    InputAction move;
    InputAction roll;
    InputAction lightAtk;
    InputAction heavyAtkCharge;
    InputAction rewind;


    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    Collider swordCollider;

    //Game Manager
    GameObject gameManager;

    //audio
    bool runningAudio;

    //camera
    GameObject camera;

    //sidecharachter
    GameObject sideCharacter;

    //interactbale objects
    GameObject interactableObj;

    //last device used variables
    InputControl lastDevice;
    public string lastDeviceStr;

    //random variable 
    int rollRandomGen;


    //for testing AI
    GameObject aiSpawner;

    #endregion

    #region - AWAKE - 
    void Awake()
    {
        
        //make this object appear at top of heirarchy in scene
        transform.SetSiblingIndex(0);

        //finding gameobjects and components
        gameManager = GameObject.Find("GameManager");
        camera = GameObject.Find("Camera");
        sideCharacter = GameObject.Find("SideCharacter");
        aiSpawner = GameObject.Find("TestArenaSpawner");
        dizzyAffect = GameObject.Find("DizzyAffect");
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        playerInput = new PlayerInputActions();

        //calling all the inputs
        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        playerInput.Player.SwitchCharacters.performed += spawnPerformed => SpawnAIButton();
        playerInput.Player.CameraRight.performed += CameraRight_performed => CameraRight();
        playerInput.Player.CameraLeft.performed += CameraLeft_performed => CameraLeft();
        playerInput.Player.Rewind.performed += rewindPerformed => Rewind();
        playerInput.Player.Attack.performed += LightAtk;
        playerInput.Player.HeavyAtkCharge.performed += HeavyAtkCharge;
        playerInput.Player.HeavyAtkRelease.performed += HeavyAtkRelease;
        playerInput.Player.Pause.performed += PressPause;
        playerInput.Player.Interact.performed += interactPerformed => Interact();

        //assigning the inputs to variables
        move = playerInput.Player.Move;
        roll = playerInput.Player.Roll;
        lightAtk = playerInput.Player.Attack;
        heavyAtkCharge = playerInput.Player.HeavyAtkCharge;
        rewind = playerInput.Player.Rewind;

        //weapons  MOVE THIS TO ANOTHER SCRIPT
        sword.GetComponent<MeshRenderer>().enabled = false;
        sword.GetComponent<BoxCollider>().enabled = false;
        sheathedSword.SetActive(true);
        swordCollider = sword.GetComponent<Collider>();
        swordCollider.enabled = false;

        baseSpeed = speed;
    }
    #endregion

    #region - START -
    private void Start()
    {
        state = PlayerState.IDLE;

        dizzyAffect.SetActive(false);

        //this is called so the rotation is checked so player doesn't roll on the spot
        rollDirection = transform.rotation * Vector3.forward;

        //create the last device container
        LastDevice();
    }

    #endregion

    #region - CHECK LAST INPUT DEVICE -
    //to find out the last input device used
    void LastDevice()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                lastDevice = lastControl.device;
                lastDeviceStr = lastDevice.ToString();

               // Debug.Log(lastDeviceStr);

               // Debug.Log($"device: {lastDevice.displayName}");  
            }
        };
    }
    #endregion

    //what does this code do?
    /*private void CameraRight_performed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
 
    }*/

    #region - UPDATE - 
    void Update()
    {
        //run last device whenever any key is pressed
        if (Input.anyKey)
        {
            LastDevice();
        }
        
        //to see state in inspector
        visibleState = state;

        PlayerMovement();

        PlayerFalling();

        RollTimer();

    }

    #endregion

    #region - MOVEMENT -
    void PlayerMovement()
    {

        //walking animation
        if(state == PlayerState.MOVING || state == PlayerState.DIZZY)
        {
            //stop moving animation while dizzy
            if(isMoving == true)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }   
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            IsometricMovement();
        }

        if(state == PlayerState.DIZZY) 
        {
            DizzyMovement();            
        }

        if (state == PlayerState.FALLING)
        {
            FallingMovement();
        }
    }

    void IsometricMovement()
    {
        speed = baseSpeed;
 
        //this code makes sure the idle state is correctly utilised
        if (isMoving == false && state == PlayerState.MOVING)
        {
            state = PlayerState.IDLE;

        }
        else if (isMoving == true && state != PlayerState.MOVING)
        {
            state = PlayerState.MOVING;

        }

        currentMoveInput = move.ReadValue<Vector2>();
        actualMovement = new Vector3();
        //Condensed movement -- Converted y to z axis
        actualMovement.x = currentMoveInput.x;
        actualMovement.z = currentMoveInput.y;

        //magic code that converts the basic player movement into isometric
        isometric = new Vector3();
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, isometricRotation, 0));
        isometric = matrix.MultiplyPoint3x4(actualMovement);

        //move the character controller
        controller.Move(isometric * speed * Time.deltaTime);
        isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

        //Character Rotation
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(isometric.x, 0, isometric.z);
        Vector3 posLookAt = currentPos + newPos;
        transform.LookAt(posLookAt);


        RollDirection();
    }

    //movement while dizzy, randomized
    void DizzyMovement()
    {
        speed = baseSpeed;

        currentMoveInput = move.ReadValue<Vector2>();
        actualMovement = new Vector3();
        //Condensed movement -- Converted y to z axis
        actualMovement.z = currentMoveInput.x;
        actualMovement.x = currentMoveInput.y;

        if (rollRandomGen > 50)
        {

            //move charachter controller
            controller.Move(actualMovement * speed * Time.deltaTime);
            isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

            //Character Rotation
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(actualMovement.x, 0, actualMovement.z);
            posLookAt = currentPos + newPos;
            transform.LookAt(posLookAt);


            RollDirection();
        }
        else
        {

            //magic code that converts the basic player movement into isometric
            isometric = new Vector3();
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, isometricRotation, 0));
            isometric = matrix.MultiplyPoint3x4(actualMovement);

            //move the character controller
            controller.Move(isometric * speed * Time.deltaTime);
            isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

            //Character Rotation
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(isometric.x, 0, isometric.z);
            Vector3 posLookAt = currentPos + newPos;
            transform.LookAt(posLookAt);

            RollDirection();
        }
    }

    //movement speed while you are falling
    void FallingMovement()
    {
        speed = baseSpeed / 4;

        currentMoveInput = move.ReadValue<Vector2>();
        actualMovement = new Vector3();
        //Condensed movement -- Converted y to z axis
        actualMovement.x = currentMoveInput.x;
        actualMovement.z = currentMoveInput.y;

        //magic code that converts the basic player movement into isometric
        isometric = new Vector3();
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, isometricRotation, 0));
        isometric = matrix.MultiplyPoint3x4(actualMovement);

        //move the character controller
        controller.Move(isometric * speed * Time.deltaTime);
        //isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

        //Character Rotation
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(isometric.x, 0, isometric.z);
        Vector3 posLookAt = currentPos + newPos;
        transform.LookAt(posLookAt);


        RollDirection();
    }
    #endregion

    #region - FALLING -
    //falling animation and detection, currently there are two sets of detection so the player can check if they're falling mutiple times
    void PlayerFalling()
    {

        //this resets when player touches ground
        if (isGrounded == true && state == PlayerState.FALLING)
        {
            state = PlayerState.IDLE;
        }

        //the below controls the players gravity, it is strong when player is grounded to prevent floating on stairs
        float weight;

        if (state != PlayerState.FALLING)
        {
            weight = -4;

            gravity = weight * Time.deltaTime;
            controller.Move(new Vector3(0, gravity, 0));
        }
        else if (state == PlayerState.FALLING || state == PlayerState.ROLLING) 
        {
            weight = 0.2f;

            //slowly increase players gravity for a more natural fall
            gravity -= weight * Time.deltaTime;
            controller.Move(new Vector3(0, gravity, 0));
        }

        float touchGround = 1.5f;

        if (Physics.CapsuleCast(transform.position - new Vector3(0f, 0f, 0f), transform.position + new Vector3(0f, 0f, 0f), controller.radius, transform.TransformDirection(Vector3.down), out RaycastHit hit, touchGround, groundMask))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);

            // little check to return to dizzy state when you land if you should be dizzy
            if(isDizzy == true)
            {
                state = PlayerState.DIZZY;
            }
        }
        else
        {

            //this if statement exist to stop the player from being knocked up and entering another state while in knockdown anim
            if(state != PlayerState.KNOCKEDDOWN)
            {

                anim.SetBool("isGrounded", false);
                
                Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);

                StartCoroutine(FallDelay());
                
            }
        }   
    }

    IEnumerator FallDelay()
    {
        yield return new WaitForSeconds(0.2f);

        isGrounded = false;

        state = PlayerState.FALLING;
    }

    #endregion

    #region - ROLL -
    //rolling animation
    void RollAnimation()
    {
        if (state == PlayerState.MOVING || state == PlayerState.IDLE)
        {
            state = PlayerState.ROLLING;

            anim.SetTrigger("Roll");
        }
    }

    //call this from other classes after a movement action occurs to make sure character knows rotation for rolling
    public void Rotation(Quaternion quaternion)
    {
        rollDirection = quaternion * Vector3.forward;
    }

    //calling this from animation state machine so it activates on state enter
    public void RollStartAnim()
    {
        StartCoroutine(RollAnim());
    }

    IEnumerator RollAnim()
    {

        rewind.Disable();
        float startTime = Time.time;

        //these variables are used for the roll timer if you roll too much you get dizzy
        rollUsed++;
        rollCDTimer = 2;

        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1f;
        while (Time.time < startTime + rollTime)
        {
            controller.Move(rollDirection * rollSpeed * Time.deltaTime);
            yield return null;

        }
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;

    }

    //calling this on animtion exit
    public void RollEndAnim()
    {

        //isRolling = false;
        if ((rollCDTimer > 0) && (rollUsed >= 3))
        {
            StartCoroutine(Dizzy());

        }
        else
        {
            rewind.Enable();
            //anim.ResetTrigger("Roll");
            state = PlayerState.IDLE;
            //StartCoroutine(EndRollWait());
        }

        //this prevents issue where attack after roll are clunky, this line prevents users from being able to instantly attack after rolling
        //instead of this we added a skip state to the animation to allow for better flow of attacking
        //yield return new WaitForSeconds(0.2f);

        //EnableLightAttack();
        //EnableHeavyAttackCharge();

    }

    //temporary fix for the animation stutter when you try to roll instantly after rolling, funnily enough you don't even notice the coroutine delay
    /*IEnumerator EndRollWait()
    {
        DisableRoll();

        yield return new WaitForSeconds(0.25f);

        EnableRoll();
    }*/

    public void RollDirection()
    {
        //set rotation for roll
       // if (isometric.magnitude >= 0.1f)
        //{
            rollDirection = transform.rotation * Vector3.forward;
        //}
    }

    void RollTimer()
    {
        //this prevents you from rolling too much
        if (rollCDTimer > 0)
        {
            rollCDTimer -= Time.deltaTime;
        }
        else if (rollCDTimer <= 0)
        {
            rollUsed = 0;
            rollCDTimer = 0;
        }
    }

    #endregion

    #region - DIZZY -
    //this affect occurs when you roll too much
    IEnumerator Dizzy()
    {
        anim.SetTrigger("Dizzy");
        dizzyAffect.SetActive(true);

        //dizzy direction changes so it's harder to learn lmao
        rollRandomGen = Random.Range(0, 101);

        state = PlayerState.DIZZY;

        isDizzy = true;

        yield return new WaitForSeconds(3);

        isDizzy = false;

        dizzyAffect.SetActive(false);

        rewind.Enable();
        anim.ResetTrigger("Roll");
        anim.SetTrigger("StopDizzy");

        if(state != PlayerState.FALLING)
        {
            state = PlayerState.IDLE;
        }
        
    }

    #endregion

    #region - LIGHT ATTACK -

    //start light attack
    void LightAtk(InputAction.CallbackContext attk)
    {
        if(state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            state = PlayerState.ATTACKING;
            GetComponent<PlayerLightAttack>().LightAtk();
        }
        
    }

    #endregion

    #region - HEAVY ATTACK -

    //Heavy attack sequence
    void HeavyAtkCharge(InputAction.CallbackContext HeavyAtkCharge)
    {
        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            state = PlayerState.ATTACKING;
            GetComponent<PlayerHeavyAttack>().HeavyAtkCharge();
        }
    }   

    //this is an input action to detect when player lets go of mouse
    void HeavyAtkRelease(InputAction.CallbackContext HeavyAtkRelease)
    {
        if(state == PlayerState.ATTACKING)
        {
            GetComponent<PlayerHeavyAttack>().HeavyAtkRelease();
        }
        
    }

    #endregion

    #region - POSSIBLE SWITCHING CHARACTER CONCEPT -
    //switch characters, after discussion we may never use this
    /*public void SwitchCharacters()
    {
        if (!isMoving)
        {
            if (!isDead)
            {
                if (!isAttacking)
                {
                    if (!isKnockdown)
                    {
                        if (!isRolling)
                        {
                            if (!isRewinding)
                            {
                                if (isGrounded)
                                {
                                    if (!isDizzy)
                                    {
                                        camera.GetComponent<CameraControls>().SwitchCharacter();

                                        DisablePlayerEnableSideCharacter();
                                    }   
                                }
                            }
                        }
                    }
                }
            }
            
        }

    }*/

    /*void DisablePlayerEnableSideCharacter()
    {
        sideCharacter.GetComponent<SideCharacterController>().enabled = true;

        *//*this.GetComponent<AttackIndicator>().enabled = false;
        this.GetComponent<PlayerKnockback>().enabled = false;
        this.GetComponent<AttackDash>().enabled = false;
        this.GetComponent<PlayerHeavyAttack>().enabled = false;
        this.GetComponent<PlayerLightAttack>().enabled = false;
        this.GetComponent<CooldownSystem>().enabled = false;
        this.GetComponent<PlayerHealth>().enabled = false;
        this.GetComponent<AttackAim>().enabled = false;
        this.GetComponent<PlayerRewind>().enabled = false;*//*
        this.GetComponent<PlayerController>().enabled = false;
    }*/

    #endregion

    #region - SPAWN AI FOR TEST ARENA -

    void SpawnAIButton()
    {
        aiSpawner.GetComponent<TestArenaSpawner>().SpawnAI();
    }

    #endregion

    #region - POSSIBLE CAMERA ROTATION CONCEPT -
    public void CameraRight()
    {
        camera.GetComponent<CameraControls>().RotateRight();
        isometricRotation = isometricRotation - 90;
    }

    public void CameraLeft()
    {
        camera.GetComponent<CameraControls>().RotateLeft();
        isometricRotation = isometricRotation + 90;
    }

    #endregion

    #region - REWIND -
    //start Rewind
    void Rewind()
    {
        if(state == PlayerState.IDLE || state == PlayerState.MOVING || state == PlayerState.FALLING || state == PlayerState.DEAD) 
        { 
            GetComponent<PlayerRewind>().PlsRewind();
        }  
    }
    #endregion

    #region - PAUSE -
    //Pause game 
    void PressPause(InputAction.CallbackContext PauseInput)
    {
        Debug.Log("pause");
        gameManager.GetComponent<GameManager>().PauseAndUnpause();
       // gameManager.GetComponent<MenuManager>().MenuUIPauseUnpause();
        
    }
    #endregion

    #region - IMMUNITY -
    //currently being used by the rewind
    public IEnumerator Immunity(float immunityTime)
    {
        immune = true;

        yield return new WaitForSeconds(immunityTime);

        immune = false;
    }
    #endregion

    #region - INTERACTING -

    public void ObtainInteractableObject(GameObject obj)
    {
        interactableObj = obj;
    }

    public void Interact()
    {
        if(inRange == true)
        {
            if(state == PlayerState.IDLE || state == PlayerState.MOVING) 
            {
                state = PlayerState.INTERACTING;
                interactableObj.gameObject.GetComponent<InteractableObject>().PressInteract(); 
            }
            else if(state == PlayerState.INTERACTING)
            {
                state = PlayerState.IDLE;
                interactableObj.gameObject.GetComponent<InteractableObject>().PressInteract();
            }
        }
    }

    #endregion

    #region - Inputs Enable/Disable -
    public void EnableMovement()
    {
        move.Enable();
    } 

    public void DisableMovement()
    {
        move.Disable();
    }

    public void EnableRoll()
    {
        roll.Enable();
    }

    public void DisableRoll()
    {
        roll.Disable();
    }

    public void EnableLightAttack()
    {
        lightAtk.Enable();
    }

    public void DisableLightAttack()
    {
        lightAtk.Disable();
    }

    public void EnableHeavyAttackCharge()
    {
        heavyAtkCharge.Enable();
    }

    public void DisableHeavyAttackCharge()
    {
        heavyAtkCharge.Disable();
    }

    public void DisableRewind()
    {
        rewind.Disable();
    }

    public void EnableRewind()
    {
        rewind.Enable();
    }

    public void EnablePlayerActionMap()
    {
        playerInput.Player.Enable();
    }

    public void DisablePlayerActionMap()
    {
        playerInput.Player.Disable();
    }

    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

    #endregion

}
