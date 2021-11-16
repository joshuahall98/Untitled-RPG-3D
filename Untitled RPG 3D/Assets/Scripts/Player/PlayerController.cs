using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //Dash
    public float dashSpeed;
    public float startDashCD;
    [SerializeField] private float dashCD;



    //HealthBar
    public Image HealthBar;
    public float startHealth = 100f;
    private float health;


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
        if (Rewinding)
        {
            OnDisable();
            Rewind();

        }
        else
        {
            Record();
            OnEnable();

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
