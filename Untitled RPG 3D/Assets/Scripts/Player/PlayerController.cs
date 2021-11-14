using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    private CharacterController controller;


    [SerializeField] Vector3 playerMoveInput;
    private float startSpeed = 7f;
    [SerializeField] private float speed;

    public PlayerInputActions playerInput;
  



    //melee attack
    [SerializeField] private float attackCD;
    public float startAttackCD;
    public Transform attackPos;
    public float attackRange;

    public LayerMask Enemey;
    public int dmg;

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;
    public float timeToRewind = 5f;

    //Dash
    private Boolean isDashing;


    //HealthBar
    public Image HealthBar;
    public float startHealth = 100f;
    private float health;




    void Awake()
    {


        health = startHealth;
        pointsInTime = new List<PointInTime>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Attack.started += attackPerformed => lightAtk();
        playerInput.Player.Rewind.performed += jumpPerformed => plsRewind();
        playerInput.Player.Dash.performed += dashPerformed => Dash();





    }

    void Update()
    {
     
        //Condensed movement
        Vector2 inputMovement = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 actualMovement = new Vector3
        {
            x = inputMovement.x,
            z = inputMovement.y
        };

        controller.Move(actualMovement * speed * Time.deltaTime);


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

        // OLD MOVEMENT CODE 
        /*    playerInput.Player.Move.performed += movementPerformed =>
            {
                playerMoveInput = new Vector3(movementPerformed.ReadValue<Vector2>().x, playerMoveInput.y, movementPerformed.ReadValue<Vector2>().y);

                float targetAngle = Mathf.Atan2(movementPerformed.ReadValue<Vector2>().x, movementPerformed.ReadValue<Vector2>().y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            };
            playerInput.Player.Move.canceled += movementPerformed =>
            {
                playerMoveInput = new Vector3(movementPerformed.ReadValue<Vector2>().x, playerMoveInput.y, movementPerformed.ReadValue<Vector2>().y);

                float targetAngle = Mathf.Atan2(movementPerformed.ReadValue<Vector2>().x, movementPerformed.ReadValue<Vector2>().y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            };
        */

        if (isDashing)
        {
            Dash();
        }
        

        //Rewind
        if (Rewinding)
        {

            Rewind();

        }
        else
        {
            Record();

        }

        void Record()
        {
            //Keep log of the last 5 seconds delete everything else
            if (pointsInTime.Count > Mathf.Round(timeToRewind / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        }
        void Rewind()
        {
            if (pointsInTime.Count > 0)
            {
                PointInTime pointInTime = pointsInTime[0];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;
                pointsInTime.RemoveAt(0);
                

            }
            else
            {
               
                Rewinding = false;
            }
        }

    }

    void lightAtk()
    {

        if (attackCD <= 0)
        {
           
            Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, Enemey);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
              enemiesToDamage[i].GetComponent<EnemyTest>().TakeDamage(dmg);
               
            }

           attackCD = startAttackCD;
        }

    }

    void Dash()
    {
        
            speed = 12;
            Debug.Log("speedyboi");
       
        
    }

    void plsRewind()
    {

        Rewinding = true;
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
