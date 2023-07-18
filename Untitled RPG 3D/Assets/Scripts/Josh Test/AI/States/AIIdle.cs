using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AIIdle : AIState
{
    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    //patrol
    Vector3 idleZone;
    Vector3 destPoint;
    [SerializeField]LayerMask groundLayer;
    bool walkPointSet;
    bool canWalk;
    NavMeshAgent navMeshAgent;

    GameObject player;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.Find("Player");
        IdleZone();   
    }

    public override AIState RunCurrentState()
    {
        if(stateManager.state == AIStateEnum.CHASE)//chase state
        {   
            controller.anim.SetBool("isWalking", false);
            return stateManager.chaseState;    
        }
        else if (stateManager.state == AIStateEnum.STAGGER)//stagger state
        {
            controller.anim.SetBool("isWalking", false);
            return stateManager.staggerState;
        }
        else//idle state
        { 
            IdleMovement();
            ActivateChaseState();
            return this;
        }

        
    }

    void ActivateChaseState() 
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10)
        {
            stateManager.state = AIStateEnum.CHASE;
        }
    }

    //move AI in idle zone
    void IdleMovement()
    {
        navMeshAgent.speed = 2; 

        if(!walkPointSet)
        {
            StartCoroutine(SearchForLocation());
            
        }
        if(canWalk)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(destPoint);
            controller.anim.SetBool("isWalking", true);
        }
        if (Vector3.Distance(this.transform.position, destPoint) < 1)
        {
            //navMeshAgent.SetDestination(gameObject.transform.position);
            walkPointSet = false;
            canWalk = false;
            controller.anim.SetBool("isWalking", false);
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            
        }
    }
    //search for new location to walk
    IEnumerator SearchForLocation()
    {
        float z = Random.Range(-10f, 10f);
        float x = Random.Range(-10f, 10f);

        destPoint = new Vector3(idleZone.x + x, idleZone.y, idleZone.z + z);

        if(Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;

            yield return new WaitForSeconds(Random.Range(2f, 5f));

            canWalk = true;
        }
    }

    void IdleZone()
    {
        idleZone = this.transform.position;
    }

    

}
