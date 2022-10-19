using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //THIS SCRIPT CONTROLS THE BASE PLAYER MOVEMENT, IT ALSO CONTROLS THE PLAYERS ROLL MECHANIC, THE DIZZY AFFECT, AND WHETHER OR NOT THE PLAYER IS GROUNDED

    //inputs and movement
    private CharacterController controller;
    [SerializeField] private float speed = 8;
    Vector2 currentMoveInput;
    Vector3 actualMovement;
    Vector3 isometric;
    public PlayerInputActions playerInput;

    //Animation
    Animator anim;

    //action checker
    //these action checkers are universal and other scripts must access them in the update to make sure they all correspond
    public static bool isMoving;
    public static bool isRolling;
    public static bool isAttacking;
    public static bool isDizzy;
    public static bool isGrounded;
    public static bool isDead;
    //need to add this everywhere
    public static bool isKnockdown;
    public static bool isRewinding;


    public bool isMovingPub;
    public bool isRollingPub;
    public bool isAttackingPub;
    public bool isDizzyPub;
    public bool isGroundedPub;
    public bool isKnockdownPub;
    public bool isRewindingPub;


    //Roll
    float rollCDTimer = 0;
    int rollUsed = 0;
    [SerializeField]float rollSpeed = 10;
    float rollTime = 0.5f;
    public GameObject dizzyAffect;
    

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

    //menu script
    GameObject gameManager;

    //audio
    bool runningAudio;

    //camera
    GameObject camera;

    //sidecharachter
    GameObject sideCharacter;


    void Awake()
    {
        
        gameManager = GameObject.Find("GameManager");

        camera = GameObject.Find("Camera");

        sideCharacter = GameObject.Find("SideCharacter");

        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        //calling all the inputs
        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        playerInput.Player.SwitchCharacters.performed += damagePerformed => SwitchCharacters();
        playerInput.Player.Rewind.performed += rewindPerformed => Rewind();
        playerInput.Player.Attack.performed += LightAtk;
        playerInput.Player.HeavyAtkCharge.performed += HeavyAtkCharge;
        playerInput.Player.HeavyAtkRelease.performed += HeavyAtkRelease;
        playerInput.Player.Pause.performed += PressEsc;

        //assigning the inputs to variables
        move = playerInput.Player.Move;
        roll = playerInput.Player.Roll;
        lightAtk = playerInput.Player.Attack;
        heavyAtkCharge = playerInput.Player.HeavyAtkCharge;
        rewind = playerInput.Player.Rewind;

        dizzyAffect = GameObject.Find("DizzyAffect");
        dizzyAffect.SetActive(false);

        //weapons
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        swordCollider = sword.GetComponent<Collider>();
        swordCollider.enabled = false;

    }

    void Update()
    {

        isMovingPub = isMoving;
        isRollingPub = isRolling;
        isAttackingPub = isAttacking;
        isDizzyPub = isDizzy;
        isGroundedPub = isGrounded;
        isKnockdownPub = isKnockdown;
        isRewindingPub = isRewinding;


        #region - Movement -
        //walking animation
        anim.SetBool("isMoving", isMoving);

        //action checks
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


                                }
                                else
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
                                    Vector3 posLookAt = currentPos + newPos;
                                    transform.LookAt(posLookAt);



                                }
                            }
                        }
                    }
                }
            }
        }
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

        #endregion

        //BUGS
        //isAttacking variable sometimes stays true freezing player character 
        //heavy attack charge activates but you have no weapon and can still move


        //unsure if needed, was input to stop running on spot bug
        if (isAttacking == true)
        {
            isMoving = false;
        }

        //stops bug where roll and attack activate at the same time
        if(isAttacking == true && isRolling == true)
        {
            isRolling = false;
            isAttacking = false;
        }

        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason

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



        #region - Falling -
        //falling animation and detection, currently there are two sets of detection so the player can check if they're falling mutiple times
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
        else if(Physics.Raycast(transform.position + new Vector3(distanceFromPlayerFarNeg, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround2, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(distanceFromPlayerFarNeg, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround2.distance, Color.red);
            
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayerFar), transform.TransformDirection(Vector3.down), out RaycastHit touchGround3, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, distanceFromPlayerFar), transform.TransformDirection(Vector3.down) * touchGround3.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, distanceFromPlayerFarNeg), transform.TransformDirection(Vector3.down), out RaycastHit touchGround4, touchGround))
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
            anim.SetBool("isGrounded", false);
            isGrounded = false;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);

        }

        #endregion

        //animation checks
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && isRolling == true)
        {
            RollEndAnim();
        }

        /*if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerKnockdown"))
        {
            Debug.Log("FINISHED");
        }*/

    }

    #region - Roll -
    //rolling animation, CREATED IENUMERAOTR TO STOP bug?
    void RollAnimation()
    {
        if (isMoving == true)
        {
            if (!isRolling)
            {
                if (isGrounded)
                {
                    if (!isAttacking)
                    {
                        if (!isKnockdown)
                        {
                            if (!isRewinding)
                            {
                                if (!isDizzy)
                                {
                                    isRolling = true;
                                    anim.SetTrigger("Roll");
                                    //StartCoroutine(RollAnim());
                                    //DisableHeavyAttackCharge();
                                    //DisableLightAttack();
                                }
                            }
                            
                        }
 
                    }
                }
            }
            
        }
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
                    
            controller.Move(isometric * rollSpeed * Time.deltaTime);
            yield return null;

        }
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;

    }

    void RollEndAnim()
    {

        isRolling = false;
        if ((rollCDTimer > 0) && (rollUsed == 3))
        {
            StartCoroutine(Dizzy());

        }
        else
        {
            rewind.Enable();
            anim.ResetTrigger("Roll");
   
        }

        //this prevents issue where attack after roll are clunky, this line prevents users from being able to instantly attack after rolling
        //instead of this we added a skip state to the animation to allow for better flow of attacking
        //yield return new WaitForSeconds(0.2f);

        //EnableLightAttack();
        //EnableHeavyAttackCharge();

    }

    #endregion

    #region - Dizzy -
    //this affect occurs when you roll too much
    IEnumerator Dizzy()
    {
        anim.SetTrigger("Dizzy");
        isDizzy = true;
        dizzyAffect.SetActive(true);

        yield return new WaitForSeconds(3);

        dizzyAffect.SetActive(false);
        isDizzy = false;

        rewind.Enable();
        anim.ResetTrigger("Roll");
        anim.SetTrigger("StopDizzy");
    }

    #endregion

    //start light attack
    void LightAtk(InputAction.CallbackContext attk)
    {
        GetComponent<PlayerLightAttack>().LightAtk();
    }

    //Heavy attack sequence
    void HeavyAtkCharge(InputAction.CallbackContext HeavyAtkCharge)
    {
        GetComponent<PlayerHeavyAttack>().HeavyAtkCharge();
    }

    void HeavyAtkRelease(InputAction.CallbackContext HeavyAtkRelease)
    {
        GetComponent<PlayerHeavyAttack>().HeavyAtkRelease();
    }

    //switch characters
    public void SwitchCharacters()
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

    }

    void DisablePlayerEnableSideCharacter()
    {
        sideCharacter.GetComponent<SideCharacterController>().enabled = true;

        /*this.GetComponent<AttackIndicator>().enabled = false;
        this.GetComponent<PlayerKnockback>().enabled = false;
        this.GetComponent<AttackDash>().enabled = false;
        this.GetComponent<PlayerHeavyAttack>().enabled = false;
        this.GetComponent<PlayerLightAttack>().enabled = false;
        this.GetComponent<CooldownSystem>().enabled = false;
        this.GetComponent<PlayerHealth>().enabled = false;
        this.GetComponent<AttackAim>().enabled = false;
        this.GetComponent<PlayerRewind>().enabled = false;*/
        this.GetComponent<PlayerController>().enabled = false;
    }

    //start Rewind
    void Rewind()
    {
        GetComponent<PlayerRewind>().PlsRewind();
    }

    //Pause game 
    void PressEsc(InputAction.CallbackContext PauseInput)
    {
        Debug.Log("Pause");
        gameManager.GetComponent<MenuManager>().PressEsc();
    }


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
