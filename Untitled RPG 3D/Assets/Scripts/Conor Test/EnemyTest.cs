using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    public Transform Player;
    public float speed = 5f;

    //HealthBar
    public Image EnemyHealthBar;
    public float EnemystartHealth = 100f;
    private float Enemyhealth;
    public float distFromPlayer;

    public float LineOfSightDist = 10f;
    public float Spinnyboi = 3f;

    private void Start()
    {
        Enemyhealth = EnemystartHealth;
   
        
        agent = GetComponent<NavMeshAgent>();
        UpdateDest();

    }
    // Update is called once per frame
    void Update()
    {
        if(Enemyhealth <= 0)
        {
            Destroy(gameObject);
        }


     

       /* if (distFromPlayer <= 5)
        { 
            //Follow
            transform.position = Vector3.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        else*/
       //Distance between points go back to patrol
        if(Vector3.Distance(transform.position,target) < 3)
        {
            IterateWaypointIndex();
            UpdateDest();
        }

        //Track distance from Enemy to Player
        distFromPlayer = Vector3.Distance(Player.position, transform.position);
                
    }

    //Change waypoint to go to
    void UpdateDest()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
    public void TakeDamage(float dmg)
    {
        Enemyhealth -= dmg;

        EnemyHealthBar.fillAmount = Enemyhealth / EnemystartHealth;
        Debug.Log("Bumfucked");
    }
    
    void EnemyAttk()
    {

    }
    void PlayerFound()
    {
        agent.SetDestination(Player.position);
    }
}
