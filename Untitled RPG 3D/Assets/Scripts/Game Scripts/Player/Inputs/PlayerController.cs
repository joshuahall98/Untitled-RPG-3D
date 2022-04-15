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
    [SerializeField] public static bool isMoving;
    [SerializeField] public static bool isRolling;
    [SerializeField] public static bool isAttacking;
    [SerializeField] public static bool isDizzy;
    [SerializeField] public static bool isGrounded;

    //Roll
    float rollCDTimer = 0;
    int rollUsed = 0;
    public float rollSpeed = 10;
    float rollTime = 0.5f;
    public GameObject dizzyAffect;
    

    //input actions
    InputAction move;
    InputAction roll;


    void Awake()
    {

        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        playerInput.Player.TakeDamageTest.performed += damagePerformed => TakeDamage();

        move = playerInput.Player.Move;
        roll = playerInput.Player.Roll;

        dizzyAffect = GameObject.Find("DizzyAffect");
        dizzyAffect.SetActive(false);

    }

    void Update()
    {

        //walking animation
        anim.SetBool("isMoving", isMoving);

        //action checks
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

                        GetComponent<PlayerLightAttack>().DisableAttack();
                        GetComponent<PlayerHeavyAttack>().DisableHeavyAttackCharge();
                        GetComponent<PlayerRewind>().DisableRewind();
                        roll.Disable();
                    }
                }  
                
            }
            
        }

        //check to see if not moving, possible bug to fix to frozen running
        if (currentMoveInput.x == 0 && currentMoveInput.y == 0)
        {
            isMoving = false;
        }

        //stop the bug that caused roll to be active while not moving, possible fix
        if (!isRolling)
        {
            anim.ResetTrigger("Roll");
            PlayerHeavyAttack.isRolling = false;
            PlayerLightAttack.isRolling = false;
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

        float touchGround = 1.5f;

        //falling animation and detection
        if(Physics.Raycast(transform.position + new Vector3(1.3f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround1, touchGround))
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
        if (isMoving == true && !isRolling && isGrounded && !isAttacking && !isDizzy)
        {
            isRolling = true;
            anim.SetTrigger("Roll");
            PlayerHeavyAttack.isRolling = true;
            PlayerLightAttack.isRolling = true;
        }
    }

    //rolling method
    IEnumerator RollAnimEvent()
    {

        GetComponent<PlayerLightAttack>().DisableAttack();
        GetComponent<PlayerHeavyAttack>().DisableHeavyAttackCharge();
        GetComponent<PlayerRewind>().DisableRewind();

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

        GetComponent<PlayerLightAttack>().EnableAttack();
        GetComponent<PlayerHeavyAttack>().EnableHeavyAttackCharge();
        GetComponent<PlayerRewind>().EnableRewind();
        PlayerHeavyAttack.isRolling = false;
        PlayerLightAttack.isRolling = false;
        anim.ResetTrigger("Roll");

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
        roll.Enable();
        GetComponent<PlayerLightAttack>().EnableAttack();
        GetComponent<PlayerHeavyAttack>().EnableHeavyAttackCharge();
        GetComponent<PlayerRewind>().EnableRewind();
        anim.ResetTrigger("Roll");
        anim.SetTrigger("StopDizzy");
    }

    public void TakeDamage()
    {
        GetComponent<PlayerHealth>().InputTakeDamage();
    }

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

    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

}
