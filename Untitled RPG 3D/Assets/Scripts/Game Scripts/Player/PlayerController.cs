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

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;
    public int timeToRewind;
    public int rewindsLeft = 4;
    public float startRewindCD;
    public float rewindCD;
    public Transform aoe;
    public float aoeAttkRange;

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
        anim = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("IsWalking");
        isAttackingHash = Animator.StringToHash("IsAttacking");

        health = maxHealth;
        pointsInTime = new List<PointInTime>();

        playerInput = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        playerInput.Player.Move.performed += movementPerformed;
        playerInput.Player.Attack.started += attackPerformed => lightAtk();
        playerInput.Player.Rewind.performed += jumpPerformed => plsRewind();
        playerInput.Player.Dash.performed += dashPerformed => Dash();

    }

    void Update()
    {
        AnimationControls();
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

    void AnimationControls()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        if (isMoving && !isWalking)
        {
            anim.SetBool(isWalkingHash, true);
        }
        else if (!isMoving && isWalking)
        {
            anim.SetBool(isWalkingHash, false);
        }
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

        //Rewind CD
        if (rewindCD > 0)
        {
            rewindCD -= Time.deltaTime;
        }
        else
        {
            rewindCD = 0;
        }

        //Run rewind function if variables are met 
        if (Rewinding == true && rewindsLeft >= 0)
        {
       
            Rewind();


        }
        else
        {
            Record();

        }

        //Rewind to point x seconds based of class "PointsInTime"
        /*        void Rewind()
                {
                    //What to Rewind to
                    if (pointsInTime.Count > 0)
                    {

                     

                        PointInTime pointInTime = pointsInTime[0];
                        transform.position = pointInTime.position;
                        transform.rotation = pointInTime.rotation;
                        health = pointInTime.hp;
                        Debug.Log(pointInTime.position);
                        //  HealthBar.fillAmount = pointInTime.hb.fillAmount;
                        pointsInTime.RemoveAt(0);


                    }
                    else
                    {

                        Rewinding = false;
                    }
                }*/
        //keep track of player state for rewind
        void Record()
        {
            //Keep log of the last x seconds delete everything else
            if (pointsInTime.Count > Mathf.Round(timeToRewind / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, health));
         
        }

    }
    //rewind player to point x seconds ago
    void Rewind()
    {
        
            PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            health = pointInTime.hp;
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
            Rewinding = false;
            rewindCD = startRewindCD;
       

    }

    //Rewind button function
    void plsRewind()
    {

    if (rewindCD <= 0)
    {
       Debug.Log("Fuckyoursystem");
        Rewinding = true;
        rewindsLeft -= 1;

            rewindCD = startRewindCD;

        }

        

    }

    //Rewind AOE Attack
    void aoeAttk()
    {

        Collider[] enemiesToDamage = Physics.OverlapSphere(aoe.position, aoeAttkRange, Enemy);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyTest>().TakeDamage(dmg);

        }
    }
    void lightAtk()
    {
        bool isAttacking = anim.GetBool(isAttackingHash);
        if (attackCD <= 0)
        {
            anim.SetTrigger(isAttackingHash);


            Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, Enemy);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {

                enemiesToDamage[i].GetComponent<EnemyTest>().TakeDamage(dmg);

            }

            attackCD = startAttackCD;
        }

        else
        {
            anim.SetBool(isAttackingHash, false);

        }

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



    //Visuals for testing range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aoe.position, aoeAttkRange);

    }




}
