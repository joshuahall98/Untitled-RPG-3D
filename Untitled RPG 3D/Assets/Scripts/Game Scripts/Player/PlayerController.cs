using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{


    private CharacterController controller;


    [SerializeField] Vector3 playerMoveInput;
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
    public int rewindsLeft = 4;
    public Transform aoe;
    public float aoeAttkRange;

    //Dash
    public float dashSpeed;
    public float startDashCD;
    [SerializeField] private float dashCD;



    //HealthBar
    public Image HealthBar;
    public float startHealth = 100f;
    public float health;


    public ParticleSystem DashP;





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
        controller.SimpleMove(Vector3.forward * 0);


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

       

        //Rewind
        if (Rewinding && rewindsLeft >= 0)
        {
            
            OnDisable();
            Rewind();

        }
        else
        {
            Record();
            OnEnable();

        }

   
        void Rewind()
        {

            if (pointsInTime.Count > 0)
            {
                PointInTime pointInTime = pointsInTime[0];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;
                health = pointInTime.hp;
                pointsInTime.RemoveAt(0);

            }
            else
            {

                Rewinding = false;
            }
        }
        void Record()
        {
            //Keep log of the last x seconds delete everything else
            if (pointsInTime.Count > Mathf.Round(timeToRewind / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation,health));
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

    void aoeAttk()
    {

        Collider[] enemiesToDamage = Physics.OverlapSphere(aoe.position, aoeAttkRange, Enemey);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyTest>().TakeDamage(dmg);

        }
    }



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

    void plsRewind()
    {

        Rewinding = true;
        rewindsLeft -= 1;
    }



    private void OnEnable()
    {

        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        Debug.Log("PlayerTookDmg");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aoe.position, aoeAttkRange);

    }




}
