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

    //melee attack
    [SerializeField] private float attackCD;
    public float startAttackCD;
    public Transform attackPos;
    public float attackRange;

    public LayerMask Enemy;
    public int dmg;

    //Dash
    public float dashSpeed;
    public float startDashCD;
    [SerializeField] private float dashCD;



    //HealthBar
    public Image HealthBar;
    public float maxHealth = 100f;
    public float health;

    Vector2 currentMovInput;
    Vector3 actualMovement;

    //Optimizing
    int isWalkingHash;
    int isAttackingHash;



    void Awake()
    {

        health = maxHealth;

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Move.performed += movementPerformed;
        playerInput.Player.Attack.started += attackPerformed => lightAtk();
        playerInput.Player.Rewind.performed += jumpPerformed => callRewind();
        playerInput.Player.Dash.performed += dashPerformed => Dash();

    }

    void Update()
    {
      
        //Condensed movement -- Converted y to z axis
        controller.Move(actualMovement * speed * Time.deltaTime);
        controller.SimpleMove(Vector3.forward * 0); //Adds Gravity for some reason

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

    }
    private void FixedUpdate()
    {

        //reset basic attack cooldown
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        else
        {
            attackCD = 0;
        }

        //Dash CD
        if (dashCD > 0)
        {
            dashCD -= Time.deltaTime;
        }
        else
        {
            dashCD = 0;
        }

    }

    void lightAtk()
    {
       
        if (attackCD <= 0)
        {
          


            Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, Enemy);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {

                enemiesToDamage[i].GetComponent<HealthSystem>().Damage(dmg);

            }

            attackCD = startAttackCD;
        }

        else
        {
           

        }

    }

    //Call PlayerRewind script
    void callRewind() {

        GetComponent<PlayerRewind>().PlsRewind();
    }
    
    //Dash button function
    void Dash()
    {

        if (dashCD <= 0)
        {
            Vector2 inputMovement = playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 actualMovement = new Vector3
            {
                x = inputMovement.x,
                z = inputMovement.y
            };
            controller.Move(actualMovement * dashSpeed * Time.deltaTime);
            Debug.Log("I wanna Dash");
            dashCD = startDashCD;

        }

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
