using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    //Animator
    private Animator anim;

    public Transform HealthBarPrefab;
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    public Transform Player;
    public float speed = 5f;

    public float distFromPlayer;

    public float LineOfSightDist = 10f;
    public float Spinnyboi = 3f;

    private void Start()
    {
        anim = GetComponent<Animator>();

        //Declaring Health + HealthBar prefab
        HealthSystem healthSystem = new HealthSystem(100);
        Transform healthBarTansform = Instantiate(HealthBarPrefab, new Vector3(0, 10), Quaternion.identity);
        HealthBar healthbar = healthBarTansform.GetComponent<HealthBar>();
        healthbar.StartHealthSystem(healthSystem); 

        //NavMesh
        agent = GetComponent<NavMeshAgent>();
    //    UpdateDest();

    }
    // Update is called once per frame
    void Update()
    {
                
    
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

    void PlayerFound()
    {
        agent.SetDestination(Player.position);
    }
}
