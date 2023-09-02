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
using static AIController;

//Joshua

//THIS SCRIPT CONTROLS THE PLAYER STATES AND ALL PLAYER CONTROLS
public enum PlayerState { IDLE, MOVING, ROLLING, ATTACKING, DEAD, REWINDING, KNOCKEDDOWN, FALLING, INTERACTING}
public enum PlayerAffect { NONE, DIZZY }

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

    //used for turning camera, need to move this off the player controller
    [SerializeField]int isometricRotation = 45;

    //Animator
    PlayerAnimController anim;

    //States
    public static PlayerState state;
    public static PlayerAffect affect;
    [SerializeField]PlayerState visibleState;

    //these bools are helpful for animations and state control
    [SerializeField]bool isMoving;
    public bool canMove;
    bool isGrounded;
    bool isDizzy;
    [SerializeField] bool isRolling;

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
        anim = this.GetComponent<PlayerAnimController>();
        controller = GetComponent<CharacterController>();

        //calling all the inputs
        playerInput = new PlayerInputActions();
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

        
    }
    #endregion

    #region - START -
    private void Start()
    {
        state = PlayerState.IDLE;
        affect = PlayerAffect.NONE;

        dizzyAffect.SetActive(false);

        //this is called so the rotation is checked so player doesn't roll on the spot
        rollDirection = transform.rotation * Vector3.forward;

        //create the last device container
        LastDevice();

        baseSpeed = speed;
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

        if(state == PlayerState.ROLLING)
        {
            StartCoroutine(RollEndAnim());
        }
        
    }

    #endregion

    #region - MOVEMENT -
    void PlayerMovement()
    {

        //check if player can move
        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        //switch state if player is moving or not
        if (isMoving && canMove && (state != PlayerState.MOVING || state == PlayerState.MOVING))
        {
            state = PlayerState.MOVING;
            anim.ChangeAnimationState(PlayerAnimController.PlayerAnimState.Run, 0.1f, 0);
        }
        else if(!isMoving && (state == PlayerState.MOVING || state == PlayerState.IDLE))
        {
            state = PlayerState.IDLE;
            anim.ChangeAnimationState(PlayerAnimController.PlayerAnimState.Idle, 0.1f, 0);
        }

        //correct movement type based off different affects
        if ((state == PlayerState.IDLE || state == PlayerState.MOVING || state == PlayerState.FALLING) && affect != PlayerAffect.DIZZY)
        {
            IsometricMovement();
        }
        else if((state == PlayerState.IDLE || state == PlayerState.MOVING || state == PlayerState.FALLING) && affect == PlayerAffect.DIZZY) 
        {
            DizzyMovement();            
        }
    }

    void IsometricMovement()
    {

        if(state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            speed = baseSpeed;
        }
        else if (state == PlayerState.FALLING)
        {
            speed = baseSpeed / 4;
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
        if(state != PlayerState.FALLING) { isMoving = (currentMoveInput.x != 0 || currentMoveInput.y != 0); }
        
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
        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            speed = baseSpeed;
        }
        else if (state == PlayerState.FALLING)
        {
            speed = baseSpeed / 4;
        }

        currentMoveInput = move.ReadValue<Vector2>();
        actualMovement = new Vector3();
        //Condensed movement -- Converted y to z axis
        actualMovement.z = currentMoveInput.x;
        actualMovement.x = currentMoveInput.y;

        //randomise the dizzy movement
        if (rollRandomGen > 50)
        {

            //move charachter controller
            controller.Move(actualMovement * speed * Time.deltaTime);
            if (state != PlayerState.FALLING) { isMoving = (currentMoveInput.x != 0 || currentMoveInput.y != 0); }

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
            if (state != PlayerState.FALLING) { isMoving = (currentMoveInput.x != 0 || currentMoveInput.y != 0); }

            //Character Rotation
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(isometric.x, 0, isometric.z);
            Vector3 posLookAt = currentPos + newPos;
            transform.LookAt(posLookAt);

            RollDirection();
        }
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
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);

            // little check to return to dizzy state when you land if you should be dizzy
            if(isDizzy == true)
            {
                affect = PlayerAffect.DIZZY;
            }
        }
        else
        {
            //this if statement exist to stop the player from being knocked up and entering another state while in knockdown anim
            if(state != PlayerState.REWINDING && state != PlayerState.KNOCKEDDOWN)
            {
                anim.ChangeAnimationState(PlayerAnimController.PlayerAnimState.Falling, 0.1f, 0);

                Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);

                StartCoroutine(FallDelay());

                state = PlayerState.FALLING;
            }
        }   
    }

    IEnumerator FallDelay()
    { 
        yield return new WaitForSeconds(0.2f);

        isGrounded = false;
    }

    #endregion

    #region - ROLL -
    //rolling animation
    void RollAnimation()
    {
        roll.Disable();

        if ((state == PlayerState.MOVING || state == PlayerState.IDLE) && affect == PlayerAffect.NONE)
        {
            state = PlayerState.ROLLING;
            anim.ChangeAnimationState(PlayerAnimController.PlayerAnimState.Roll, 0.1f, 0);
            StartCoroutine(RollAnim());    
        }
        else
        {
            roll.Enable();
        }
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
            controller.Move(rollDirection * rollSpeed * Time.deltaTime);//don't use fixed dealt time in coroutine
            yield return null;
        }
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
    }

    //calling this when animation finishes playing
    IEnumerator RollEndAnim()
    {
        if(anim.IsAnimationDone(PlayerAnimController.PlayerAnimState.Roll))
        {
            if ((rollCDTimer > 0) && (rollUsed >= 3))
            {
                StartCoroutine(Dizzy());
            }
            else
            {
                rewind.Enable();
                state = PlayerState.IDLE;
                yield return new WaitForSeconds(0.1f);
                roll.Enable();
            }
        }
    }

    //call this from other classes after a movement action occurs to make sure character knows rotation for rolling
    public void Rotation(Quaternion quaternion)
    {
        rollDirection = quaternion * Vector3.forward;
    }


    public void RollDirection()
    {
        rollDirection = transform.rotation * Vector3.forward;
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
        if (state != PlayerState.FALLING)
        {
            //anim.anim.Rebind();

            state = PlayerState.IDLE;
        }

        anim.ChangeAnimationAffectState(PlayerAnimController.PlayerAnimAffect.Dizzy, 0.1f, 1);
        //anim.SetTrigger("Dizzy");
        dizzyAffect.SetActive(true);

        //dizzy direction changes so it's harder to learn lmao
        rollRandomGen = Random.Range(0, 101);

        affect = PlayerAffect.DIZZY;

        isDizzy = true;

        yield return new WaitForSeconds(3);

        anim.ChangeAnimationAffectState(PlayerAnimController.PlayerAnimAffect.None, 0.1f, 1);

        isDizzy = false;

        dizzyAffect.SetActive(false);

        rewind.Enable();
        // anim.ResetTrigger("Roll");
        // anim.SetTrigger("StopDizzy");

        affect = PlayerAffect.NONE;

        roll.Enable();
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
        //Debug.Log("pause");
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
