using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
    [SerializeField] float rollCDTimer = 0;
    [SerializeField] int rollUsed = 0;
    public float rollSpeed = 10;
    public float rollTime = 0.5f;
    public GameObject dizzyAffect;
    

    //input actions
    InputAction move;
    InputAction roll;

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    

    void Awake()
    { 
        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Attack.performed += lightAtk;
        playerInput.Player.HeavyAttk.performed += HeavyAtk;
        playerInput.Player.Roll.performed += rollPerformed => RollAnimation();
        playerInput.Player.TakeDamageTest.performed += damagePerformed => TakeDamage();

        move = playerInput.Player.Move;
        roll = playerInput.Player.Roll;

        dizzyAffect = GameObject.Find("DizzyAffect");
        dizzyAffect.SetActive(false);


        //weapons
        sword.SetActive(false);
        sheathedSword.SetActive(true);

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
                    }
                }
                
                
            }
            
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
        if(Physics.Raycast(transform.position + new Vector3(1.5f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround1, 1.4f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(1.5f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround1.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(-1.5f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround2, 1.4f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(-1.5f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround2.distance, Color.red);
            
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, 1.5f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround3, 1.4f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 1.5f), transform.TransformDirection(Vector3.down) * touchGround3.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, -1.5f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround4, 1.4f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, -1.5f), transform.TransformDirection(Vector3.down) * touchGround4.distance, Color.red);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            isGrounded = false;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 0f), transform.TransformDirection(Vector3.down) * 1f, Color.green);

        }

    }

    //rolling animation
    void RollAnimation()
    {
        anim.SetTrigger("Roll");
    }

    //rolling method
    IEnumerator RollAnimEvent()
    {
        if(isMoving == true && !isRolling  && isGrounded)
        {

            isRolling = true;

            
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
    }

    void RollEndAnimEvent()
    {
        isRolling = false;
        if ((rollCDTimer > 0) && (rollUsed == 3))
        {
            StartCoroutine(Dizzy());
        }
    }

    //this affect occurs when you roll too much
    IEnumerator Dizzy()
    {
        anim.SetTrigger("Dizzy");
        isDizzy = true;
        roll.Disable();
        GetComponent<PlayerRewind>().DisableRewind();
        dizzyAffect.SetActive(true);

        yield return new WaitForSeconds(3);

        dizzyAffect.SetActive(false);
        isDizzy = false;
        roll.Enable();
        GetComponent<PlayerRewind>().EnableRewind();
    }

    void lightAtk(InputAction.CallbackContext attk)
    {
        anim.SetTrigger("LightAttack");
        //StartCoroutine(LightAttackAction());
  
    }

    //starting the attack animation
    void LightAttackStartAnimEvent()
    {
        isAttacking = true;
        WeaponDamage.isAttacking = true;
        sheathedSword.SetActive(false);
        sword.SetActive(true);
        
    }

    //ending the attack animation
    void LightAttackEndAnimEvent()
    {
        isAttacking = false;
        WeaponDamage.isAttacking = false;
        sheathedSword.SetActive(true);
        sword.SetActive(false);
    }

    
    void HeavyAtk(InputAction.CallbackContext HeavyAttk)
    {
        Debug.Log("HeavyAtk");
        //anim.SetTrigger("HeavyAttk");
    }


    public void PlayerDeath()
    {
        Debug.Log("Player ded");
        move.Disable();
        anim.Play("PlayerDeath");
        
    }
    public void TakeDamage()
    {
        GetComponent<PlayerHealth>().InputTakeDamage();
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
