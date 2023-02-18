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


//THIS SCRIPT CONTROLS THE PLAYER STATES AND ALL PLAYER CONTROLS
public enum PlayerState { IDLE, MOVING, ROLLING, ATTACKING, DEAD, REWINDING, DIZZY, KNOCKEDDOWN, FALLING, INTERACTING}
public class PlayerController : MonoBehaviour
{
    
    //inputs and movement
    private CharacterController controller;
    [SerializeField] private float speed = 8;
    Vector2 currentMoveInput;
    Vector3 actualMovement;
    [SerializeField]Vector3 isometric;
    Vector3 posLookAt;
    public PlayerInputActions playerInput;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

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

    //to check whether or not an object is within range for interaction
    public static bool inRange =  false;

    //to check if player character is immune
    public static bool immune = false;

    //Roll
    float rollCDTimer = 0;
    int rollUsed = 0;
    [SerializeField]float rollSpeed = 10;
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

    //last device used variable
    public static InputControl lastDevice;


    void Awake()
    {
        //make this object appear at top of heirarchy
        transform.SetSiblingIndex(0);

        //finding gameobjects and components
        gameManager = GameObject.Find("GameManager");
        camera = GameObject.Find("Camera");
        sideCharacter = GameObject.Find("SideCharacter");
        anim = GetComponent<Animator>();
        playerInput = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
        dizzyAffect = GameObject.Find("DizzyAffect");
        dizzyAffect.SetActive(false);

        //calling all the inputs
        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        //playerInput.Player.SwitchCharacters.performed += damagePerformed => SwitchCharacters();
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

        //weapons
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        swordCollider = sword.GetComponent<Collider>();
        swordCollider.enabled = false;

        state = PlayerState.IDLE;

        //this is called so the rotation is checked so player doesn't roll on the spot
        rollDirection = transform.rotation * Vector3.forward;

    }

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

                //Debug.Log($"device: {lastDevice.displayName}");

            
            }
        };
    }

    //what does this code do?
    private void CameraRight_performed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        //to see state in inspector
        visibleState = state;

        PlayerMovement();

        PlayerFalling();

        LastDevice();


        /*if (state == PlayerState.ROLLING)
        {
            StartCoroutine(RollMovement());
        }*/


        //running sound
        //is currently broken unsure why, need to remake it
        /*if (isMoving && isGrounded)
        {
            FindObjectOfType<SoundManager>().PlaySound("Running");
        }
        else
        {   
            FindObjectOfType<SoundManager>().StopSound("Running");
        }*/

        //BUGS
        //isAttacking variable sometimes stays true freezing player character 
        //heavy attack charge activates but you have no weapon and can still move


        //unsure if needed, was input to stop running on spot bug
        /*if (isAttacking == true)
        {
            isMoving = false;
        }*/

        //stops bug where roll and attack activate at the same time
        /*if(isAttacking == true && isRolling == true)
        {
            isRolling = false;
            isAttacking = false;
        }*/


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

        //animation checks
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && state == PlayerState.ROLLING)
        {
            RollEndAnim();
        }

        /*if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerKnockdown"))
        {
            Debug.Log("FINISHED");
        }*/

    }

    #region - MOVEMENT -
    void PlayerMovement()
    {
        //walking animation
        anim.SetBool("isMoving", isMoving);


        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {

            //this code makes sure the idle state is correctly utilised
            if (isMoving == false && state == PlayerState.MOVING)
            {
                state = PlayerState.IDLE;

            }
            else if (isMoving == true && state != PlayerState.MOVING)
            {
                state = PlayerState.MOVING;
                
            }


            //this old code unsure what it does
            /*Vector2 readVector = move.ReadValue<Vector2>();
            Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y);
            currentMoveInput = IsoVectorConvert(toConvert);

            controller.Move(currentMoveInput * speed * Time.deltaTime);*/

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

            //set rotation for roll
            if (isometric.magnitude >= 0.1f)
            {
                rollDirection = transform.rotation * Vector3.forward;
            }
            
            


        }

        if(state == PlayerState.DIZZY) 
        {
            currentMoveInput = move.ReadValue<Vector2>();
            actualMovement = new Vector3();
            //Condensed movement -- Converted y to z axis
            actualMovement.z = currentMoveInput.x;
            actualMovement.x = currentMoveInput.y;

            //move charachter controller
            controller.Move(actualMovement * speed * Time.deltaTime);
            isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;


            //Character Rotation
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(actualMovement.x, 0, actualMovement.z);
            posLookAt = currentPos + newPos;
            transform.LookAt(posLookAt);

            //set rotation for roll
            if (actualMovement.magnitude >= 0.1f)
            {
                rollDirection = transform.rotation * Vector3.forward;
            }
        }


    }

    //BRACKEYS VID, REMADE BUT BETTER
    //this was created so that the rotation of the player can be called after different actions, prevents rolling the wrong way
    /*public void Rotation(Vector3 vector3)
    {
        float targetAngle = Mathf.Atan2(vector3.x, vector3.z) * Mathf.Rad2Deg;
        //this line dampens the rotation
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        rollDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }*/

    #endregion

    #region - Falling -
    //falling animation and detection, currently there are two sets of detection so the player can check if they're falling mutiple times
    void PlayerFalling()
    {

        //this resets when player touches ground
        if (isGrounded == true && state == PlayerState.FALLING)
        {
            state = PlayerState.IDLE;
        }

        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason

        float touchGround = 2f;
        float distanceFromPlayerFar = 1.3f;
        float distanceFromPlayerFarNeg = -1.3f;
        float distanceFromPlayer = 0.5f;
        float distanceFromPlayerNeg = -0.5f;

        if (Physics.Raycast(transform.position + new Vector3(distanceFromPlayerFar, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround1, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(distanceFromPlayerFar, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround1.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(distanceFromPlayerFarNeg, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround2, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(distanceFromPlayerFarNeg, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround2.distance, Color.red);

        }
        else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayerFar), transform.TransformDirection(Vector3.down), out RaycastHit touchGround3, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, distanceFromPlayerFar), transform.TransformDirection(Vector3.down) * touchGround3.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayerFarNeg), transform.TransformDirection(Vector3.down), out RaycastHit touchGround4, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, distanceFromPlayerFarNeg), transform.TransformDirection(Vector3.down) * touchGround4.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(distanceFromPlayer, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround5, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(distanceFromPlayer, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround5.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(distanceFromPlayerNeg, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround6, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(distanceFromPlayerNeg, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround6.distance, Color.red);

        }
        else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayer), transform.TransformDirection(Vector3.down), out RaycastHit touchGround7, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, distanceFromPlayer), transform.TransformDirection(Vector3.down) * touchGround7.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayerNeg), transform.TransformDirection(Vector3.down), out RaycastHit touchGround8, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, distanceFromPlayerNeg), transform.TransformDirection(Vector3.down) * touchGround8.distance, Color.red);
        }
        else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround9, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround9.distance, Color.red);

        }
        else
        {
            
            //this if statement exist to stop the player from being knocked up and entering another state while in knockdown anim
            if(state != PlayerState.KNOCKEDDOWN)
            {

                anim.SetBool("isGrounded", false);
                isGrounded = false;

                state = PlayerState.FALLING;

                Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);
            }

        }

        
    }
    #endregion

    #region - ROLL -
    //rolling animation, CREATED IENUMERAOTR TO STOP bug?
    void RollAnimation()
    {
        if(state == PlayerState.MOVING  || state == PlayerState.IDLE)
        {
            state = PlayerState.ROLLING;

            anim.SetTrigger("Roll");
        }
    }

    //call this from other classes to make sure character knows rotation for rolling
    public void Rotation(Quaternion quaternion)
    {
        rollDirection = quaternion * Vector3.forward;
    }

    //running the below function on the first frame of the animation to prevent the character from dashing before rolling
    IEnumerator RollAnimEvent()
    {

        rewind.Disable();
        float startTime = Time.time;

        //these variables are used for the roll timer if you roll too much
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

    void RollEndAnim()
    {

        //isRolling = false;
        if ((rollCDTimer > 0) && (rollUsed == 3))
        {
            StartCoroutine(Dizzy());

        }
        else
        {
            rewind.Enable();
            anim.ResetTrigger("Roll");
            state = PlayerState.IDLE;
        }

        //this prevents issue where attack after roll are clunky, this line prevents users from being able to instantly attack after rolling
        //instead of this we added a skip state to the animation to allow for better flow of attacking
        //yield return new WaitForSeconds(0.2f);

        //EnableLightAttack();
        //EnableHeavyAttackCharge();

    }

    #endregion

    #region - DIZZY -
    //this affect occurs when you roll too much
    IEnumerator Dizzy()
    {
        anim.SetTrigger("Dizzy");
        //isDizzy = true;
        dizzyAffect.SetActive(true);

        state = PlayerState.DIZZY;

        yield return new WaitForSeconds(3);

        dizzyAffect.SetActive(false);
        //isDizzy = false;

        rewind.Enable();
        anim.ResetTrigger("Roll");
        anim.SetTrigger("StopDizzy");

        state = PlayerState.IDLE;
    }

    #endregion

    //start light attack
    void LightAtk(InputAction.CallbackContext attk)
    {
        if(state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            state = PlayerState.ATTACKING;
            GetComponent<PlayerLightAttack>().LightAtk();
            // to stop running anim 
            isMoving = false;
        }
        
    }

    //Heavy attack sequence
    void HeavyAtkCharge(InputAction.CallbackContext HeavyAtkCharge)
    {
        if (state == PlayerState.IDLE || state == PlayerState.MOVING)
        {
            state = PlayerState.ATTACKING;
            GetComponent<PlayerHeavyAttack>().HeavyAtkCharge();
            // to stop running anim 
            isMoving = false;
        }
    }

    void HeavyAtkRelease(InputAction.CallbackContext HeavyAtkRelease)
    {
        if(state == PlayerState.ATTACKING)
        {
            GetComponent<PlayerHeavyAttack>().HeavyAtkRelease();
        }
        
    }

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

    //start Rewind
    void Rewind()
    {
        if(state == PlayerState.IDLE || state == PlayerState.MOVING || state == PlayerState.FALLING || state == PlayerState.DEAD) 
        { 
            GetComponent<PlayerRewind>().PlsRewind();
        }  
    }

    //Pause game 
    void PressPause(InputAction.CallbackContext PauseInput)
    {
        //this code to call game manager is bettert
        //GameManager.instance.PauseAndUnpause();
        gameManager.GetComponent<GameManager>().PauseAndUnpause();
        gameManager.GetComponent<MenuManager>().MenuUIPauseUnpause();
        
    }

    public IEnumerator Immunity(float immunityTime)
    {
        immune = true;

        yield return new WaitForSeconds(immunityTime);

        immune = false;
    }

    #region - Interacting -

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
                // to stop running anim when interacting
                isMoving = false; 
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
