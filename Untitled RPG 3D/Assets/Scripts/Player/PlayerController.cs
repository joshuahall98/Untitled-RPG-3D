using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] Vector3 playerMoveInput;
    [SerializeField] private float speed;

    public PlayerInputActions playerInput;

    //melee attack
    [SerializeField]private float attackCD;
    public float startAttackCD;
    public Transform attackPos;
    public float attackRange;

    public LayerMask Enemey;
    public int dmg;
    

    private void FixedUpdate()
    {
        
        //reset basic attack cooldown
        if(attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        else
        {
            attackCD = 0;
        }

        //running the player movement
        playerInput.Player.Move.performed += movementPerformed =>
        {
            playerMoveInput = new Vector3(movementPerformed.ReadValue<Vector2>().x, playerMoveInput.y, movementPerformed.ReadValue<Vector2>().y);
        };
        playerInput.Player.Move.canceled += movementPerformed =>
        {
            playerMoveInput = new Vector3(movementPerformed.ReadValue<Vector2>().x, playerMoveInput.y, movementPerformed.ReadValue<Vector2>().y);
        };

        Movement();


    }
    void Awake()
    {
        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Attack.started += attackPerformed => lightAtk();
        //playerInput.Player.Move.performed += movementPerformed => Movement();
        playerInput.Player.Dash.performed += jumpPerformed => Dash();

    }


    void lightAtk()
    {
       
        if (attackCD <= 0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemey);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyTest>().takeDamage(dmg);

            }

            attackCD = startAttackCD;
        }
        
    }

    void Movement()
    {
        
        Vector3 MoveVec = transform.TransformDirection(playerMoveInput);

        controller.Move(MoveVec * speed * Time.deltaTime);

        controller.SimpleMove(Vector3.forward * 0);
    }

    void Dash()
    {
        Debug.Log("RolliePollie");
    }

    private void OnEnable()
    {
  
        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
