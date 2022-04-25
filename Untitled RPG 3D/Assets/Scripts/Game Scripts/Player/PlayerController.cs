using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    //THIS SCRIPT CONTROLS THE BASE PLAYER MOVEMENT, IT ALSO CONTROLS THE PLAYERS ROLL MECHANIC, THE DIZZY AFFECT, AND WHETHER OR NOT THE PLAYER IS GROUNDED

    //inputs and movement
    private CharacterController controller;
    [SerializeField] private float speed = 8;
    Vector2 currentMoveInput;
    Vector3 actualMovement;
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

    public bool isMovingPub;
    public bool isRollingPub;
    public bool isAttackingPub;
    public bool isDizzyPub;
    public bool isGroundedPub;

    //Roll
    float rollCDTimer = 0;
    int rollUsed = 0;
    public float rollSpeed = 10;
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
    public GameObject gameManager;


    void Awake()
    {
        gameManager = GameObject.Find("GameManager");

        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        //calling all the inputs
        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        playerInput.Player.TakeDamageTest.performed += damagePerformed => TakeDamage();
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

        //walking animation
        anim.SetBool("isMoving", isMoving);

        //action checks
        if (!isDead)
        {
            if (!isAttacking)
            {
                if (!isRolling)
                {
                    if (isGrounded)
                    {
                        if (!isDizzy)
                        {
                            currentMoveInput = move.ReadValue<Vector2>();
                            actualMovement = new Vector3();
                            //Condensed movement -- Converted y to z axis
                            actualMovement.x = currentMoveInput.x;
                            actualMovement.z = currentMoveInput.y;
                            controller.Move(actualMovement * speed * Time.deltaTime);
                            isMoving = currentMoveInput.x != 0 || currentMoveInput.y != 0;

                            //Character Rotation
                            Vector3 currentPos = transform.position;

                            Vector3 newPos = new Vector3(actualMovement.x, 0, actualMovement.z);
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
        
        //BUGS
        //isAttacking variable sometimes stays true freezing player character 
        //heavy attack charge activates but you have no weapon and can still move


        //unsure if needed, was input to stop running on spot bug
        if(isAttacking == true)
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



        //falling animation and detection
        float touchGround = 2f;

        if (Physics.Raycast(transform.position + new Vector3(1.3f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround1, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(1.3f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround1.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(-1.3f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround2, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(-1.3f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround2.distance, Color.red);
            
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, 1.3f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround3, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 1.3f), transform.TransformDirection(Vector3.down) * touchGround3.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, -1.3f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround4, touchGround))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, -1.3f), transform.TransformDirection(Vector3.down) * touchGround4.distance, Color.red);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            isGrounded = false;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);

        }

    }

    //rolling animation, CREATED IENUMERAOTR TO STOP 
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
                        if (!isDizzy)
                        {
                            isRolling = true;
                            anim.SetTrigger("Roll");
                        }
                    }
                }
            }
            
        }
    }

    //rolling method
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
                    
            controller.Move(actualMovement * rollSpeed * Time.deltaTime);
            yield return null;

        }
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;

    }

    void RollEndAnimEvent()
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

    }

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

    //testing take damage
    public void TakeDamage()
    {
        GetComponent<PlayerHealth>().InputTakeDamage();
    }

    //start Rewind
    void Rewind()
    {
        GetComponent<PlayerRewind>().PlsRewind();
    }

    //Pause game 
    void PressEsc(InputAction.CallbackContext PauseInput)
    {
        gameManager.GetComponent<MenuManager>().PressEsc();
    }


    //EVERYTHING BELOW THIS COMMENT DISABLES AND ENABLES INPUTS
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

    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

}
