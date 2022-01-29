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
    [SerializeField] public static bool isMoving;
    [SerializeField] public static bool isRolling;
    [SerializeField] public static bool isAttacking;
    [SerializeField] public static bool isDizzy;
    [SerializeField] public static bool isGrounded;

    public LayerMask Enemy;
    public int dmg;

    //Roll
    [SerializeField] float rollCDTimer = 0;
    [SerializeField] int rollUsed = 0;
    public float rollSpeed = 20;
    public float rollTime = 0.5f;
    public GameObject dizzyAffect;
    
    //HealthBar
    public Image HealthBar;
    public float maxHealth = 100f;
    public float health;

    //input actions
    InputAction move;
    InputAction roll;
    

    void Awake()
    { 
        anim = GetComponent<Animator>();

        health = maxHealth;

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Attack.performed += lightAtk;
        playerInput.Player.HeavyAttk.performed += HeavyAtk;
        playerInput.Player.Roll.performed += rollPerformed => StartCoroutine(Roll());
        playerInput.Player.TakeDamageTest.performed += damagePerformed => TakeDamage();

        move = playerInput.Player.Move;
        roll = playerInput.Player.Roll;

        dizzyAffect = GameObject.Find("DizzyAffect");
        dizzyAffect.SetActive(false);

    }

    void Update()
    {
        
        //action checks
        if (!isAttacking) 
        {
            if (!isRolling)
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

        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason

        //walking animation
        anim.SetBool("isMoving", isMoving);

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
        if(Physics.Raycast(transform.position + new Vector3(1.5f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround1, 2f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(1.5f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround1.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(-1.5f, 0f, 0f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround2, 2f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(-1.5f, 0f, 0f), transform.TransformDirection(Vector3.down) * touchGround2.distance, Color.red);
            
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, 1.5f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround3, 2f))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            Debug.DrawRay(transform.position + new Vector3(0f, 0f, 1.5f), transform.TransformDirection(Vector3.down) * touchGround3.distance, Color.red);
        }
        else if(Physics.Raycast(transform.position + new Vector3(0f, 0f, -1.5f), transform.TransformDirection(Vector3.down), out RaycastHit touchGround4, 2f))
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

    //rolling method
    IEnumerator Roll()
    {
        if(isMoving == true && !isRolling  && isGrounded)
        {

            isRolling = true;

            anim.SetTrigger("Roll");
            float startTime = Time.time;
                
            //these variables are used for the roll timer if you roll too much
            rollUsed++;
            rollCDTimer = 2;
                
            controller.center = new Vector3(0, -0.5f, 0);
            controller.height = 2.5f;
            while (Time.time < startTime + rollTime)
            {
                    
                controller.Move(actualMovement * rollSpeed * Time.deltaTime);
                yield return null;

            }
            controller.center = new Vector3(0, 0, 0);
            controller.height = 4;
            isRolling = false;
            if ((rollCDTimer > 0) && (rollUsed == 3))
            {
                StartCoroutine(Dizzy());
            }

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
        Debug.Log("LightAttack");
        StartCoroutine(LightAttackAction());
        
        
    }

    IEnumerator LightAttackAction()
    {
        isAttacking = true;
        JAHPlayerPunch.attacking = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
        JAHPlayerPunch.attacking = false;
    }

    void HeavyAtk(InputAction.CallbackContext HeavyAttk)
    {
        Debug.Log("HeavyAtk");
        //anim.SetTrigger("HeavyAttk");

    }

    public void PlayerTakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log("PlayerTookDamage");
    }

     

   /* //Visuals for testing range
    private void OnDrawGizmosSelected()
    {   
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aoe.position, aoeAttkRange);

    }
   */

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
