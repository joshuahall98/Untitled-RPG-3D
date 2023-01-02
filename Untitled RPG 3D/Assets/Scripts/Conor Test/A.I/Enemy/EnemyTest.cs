using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour, CooldownActive
{

    //Cooldown
    [SerializeField] private CooldownSystem cooldownSystem;
    private string Id = "RangedAttk";
    [SerializeField] private float CooldownDuration = 2;
    public string id => Id;
    public float cooldownDuration => CooldownDuration;

    //EnemyStats from Scriptable Object
    public EnemyScriptableObject Enemy;

    //Animator
    private Animator anim;
    public Transform[] waypoints;
    public Transform HealthBarPrefab;
    public GameObject Player;
    public float speed;
    public NavMeshAgent agent;

    public float distFromPlayer;
    public float retreatDistance;
    public float attackRadius;
    public float stoppingDistance;
    public float AISpace;
    
    bool isPlayerDead;

    public GameObject[] AI;



    private void Start()
    {
        anim = GetComponent<Animator>();

        //NavMesh
        agent = GetComponent<NavMeshAgent>();


           Player = GameObject.FindGameObjectWithTag("Player");
           attackRadius = Enemy.attackRadius;
           speed = Enemy.speed;


        AI = GameObject.FindGameObjectsWithTag("Enemy");

        agent.speed = speed;

                

    }
    // Update is called once per frame
    void Update()
    {


        isPlayerDead = PlayerHealth.isDead;

        if (isPlayerDead == true)
        {
            PlayerDead();
        }

        //Track distance from Enemy to Player
        distFromPlayer = Vector3.Distance(Player.transform.position, transform.position);


        //Seperation
        foreach (GameObject go in AI)
        {
            if (go != gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, this.transform.position);
                if (distance <= AISpace)
                {
                    Vector3 direction = transform.position - go.transform.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }


        }
    }


    public void Death()
    {
        anim.SetBool("isDead", true);
        anim.Play("Die");
        Destroy(gameObject, 10);
        GetComponent<ConXP>().DropXP();

    }
  public  void PlayerDead()
    {
        
        anim.Play("Death");
        agent.velocity = Vector3.zero;
    }
}
