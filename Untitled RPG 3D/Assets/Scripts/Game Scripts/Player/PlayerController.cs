using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    private CharacterController controller;


    [SerializeField] Vector3 playerMoveInput;
    [SerializeField] private float speed;

    public PlayerInputActions playerInput;

    //Animation
    Animator anim;

    private bool isMoving;

    public LayerMask Enemy;
    public int dmg;

    //Dash
    [SerializeField ]float rollCDTimer = 0;
    [SerializeField] int dashUsed = 0;
    public float dashSpeed;
    public float dashTime;


    //HealthBar
    public Image HealthBar;
    public float maxHealth = 100f;
    public float health;

    Vector2 currentMovInput;
    Vector3 actualMovement;





    void Awake()
    { 
        anim = GetComponent<Animator>();

        health = maxHealth;

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Move.performed += movementPerformed;
        playerInput.Player.Attack.started += lightAtk;
        playerInput.Player.HeavyAttk.started += HeavyAtk;
        playerInput.Player.Rewind.performed += jumpPerformed => callRewind();
        playerInput.Player.Dash.performed += dashPerformed => StartCoroutine(Dash());

    }

    void Update()
    {
        
        //Condensed movement -- Converted y to z axis
        controller.Move(actualMovement * speed * Time.deltaTime);
        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason

        if(rollCDTimer > 0)
        {
            rollCDTimer -= Time.deltaTime;
        }
        if((rollCDTimer > 0) && (dashUsed == 3))
        {
            //Dizzy
            playerInput.Disable();
        }
        else if(rollCDTimer <= 0)
        {
            
            dashUsed = 0;
            playerInput.Enable();
        }


    }

    void movementPerformed(InputAction.CallbackContext context)
    {

        currentMovInput = context.ReadValue<Vector2>();
        actualMovement.x = currentMovInput.x;
        actualMovement.z = currentMovInput.y;
        isMoving = currentMovInput.x != 0 || currentMovInput.y != 0;

        //Character Rotation
        Vector3 currentPos = transform.position;

        Vector3 newPos = new Vector3(actualMovement.x, 0, actualMovement.z);
        Vector3 posLookAt = currentPos + newPos;
        transform.LookAt(posLookAt);
        WalkAnim(posLookAt);

    }
    

    void WalkAnim(Vector3 newPos)
    {
        //Condensed changing bool Value based on if there is any movement on the 2 axis 
        isMoving = (actualMovement.x > 0.1f || actualMovement.x < -0.1f || actualMovement.z > 0.1f || actualMovement.z < -0.1f) ? true : false;
        anim.SetBool("isMoving", isMoving);
    }

    IEnumerator Dash()
    {
        playerInput.Disable();
        anim.SetTrigger("isRolling");
        float startTime = Time.time;

        dashUsed++;
        rollCDTimer = 2;


        controller.center = new Vector3(0, 0.5f, 0);
        controller.height = 1;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(actualMovement * dashSpeed * Time.deltaTime);
            yield return null;
        }
        controller.center = new Vector3(0, 1, 0);
        controller.height = 2;   
        playerInput.Enable();
        controller.Move(new Vector3(0, 0, 0));


    }
    void lightAtk(InputAction.CallbackContext attk)
    {

        anim.SetTrigger("isAttacking");

    }

    void HeavyAtk(InputAction.CallbackContext HeavyAttk)
    {

        anim.SetTrigger("HeavyAttk");

    }

    //Call PlayerRewind script
    void callRewind() {

        //Call Rewind Script on keypress
        GetComponent<PlayerRewind>().PlsRewind();
    }


  private void OnEnable()
    {

        playerInput.Enable();
    }

   private void OnDisable()
    {
       
        playerInput.Disable();
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



}
