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
    Transform Player;
    public float speed;
    public NavMeshAgent agent;

    public float distFromPlayer;

    public GameObject projectile;


    bool isPlayerDead;


    private void Start()
    {
        anim = GetComponent<Animator>();

        //NavMesh
        agent = GetComponent<NavMeshAgent>();

        
        Player = Enemy.Player;
        
        

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
        distFromPlayer = Vector3.Distance(Player.position, transform.position);
       

    }

    void RangedAttack()
    {
        if (cooldownSystem.IsOnCooldown(id)) { return; }
        {

            Instantiate(projectile, transform.position, Quaternion.identity);

            cooldownSystem.PutOnCooldown(this);
        }


    }


    public void Death()
    {
        anim.Play("Die");
        Destroy(gameObject, 2);
        GetComponent<ConXP>().DropXP();

    }
  public  void PlayerDead()
    {
        
        anim.Play("Victory");
    }
}
