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

    //HealthBar
    public Image EnemyHealthBar;
    public float EnemystartHealth = 100f;
    private float Enemyhealth;
   

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

        if(Vector3.Distance(transform.position,target) < 3)
        {
            IterateWaypointIndex();
            UpdateDest();
        }
                
    }

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
}
