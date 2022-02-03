using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    //Animator
    private Animator anim;
    public Transform[] waypoints;
    public Transform HealthBarPrefab;
    public Transform Player;
    public float speed = 5f;

    public float distFromPlayer;


    bool isPlayerDead;


    private void Start()
    {
        anim = GetComponent<Animator>();

        //NavMesh
     //   agent = GetComponent<NavMeshAgent>();
    //    UpdateDest();

        

    }
    // Update is called once per frame
    void Update()
    {
                
    
/*       //Distance between points go back to patrol
        if(Vector3.Distance(transform.position,target) < 3)
        {
            IterateWaypointIndex();
            UpdateDest();
        }*/

        isPlayerDead = PlayerHealth.playerIsDead;

        if (isPlayerDead == true)
        {
            PlayerDead();
        }
        //Track distance from Enemy to Player
        distFromPlayer = Vector3.Distance(Player.position, transform.position);
       

    }


    //Change waypoint to go to
  /*  void UpdateDest()
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
    }*/


 public void Death()
    {
        Debug.Log("I ded");
        anim.Play("Die");
        Destroy(gameObject, 2);
        GetComponent<ConXP>().DropXP();

    }
  public  void PlayerDead()
    {
        Debug.Log("PlayerisDead");
        anim.Play("Victory");
    }
}
