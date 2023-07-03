using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AIIdle : AIState
{
    public AIChaseState chaseState;
    public AIStaggerState staggerState;

    //state transitions
    [SerializeField]bool canSeePlayer;
    [SerializeField] bool staggered;

    Animator anim;

    //patrol
    [SerializeField]Vector3 idleZone;
    [SerializeField] Vector3 destPoint;
    [SerializeField] LayerMask groundLayer;
    [SerializeField]bool walkPointSet;
    [SerializeField] bool canWalk;
    NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponentInParent<Animator>();
        IdleZone();
    }

    public override AIState RunCurrentState()
    {
        if(canSeePlayer)//chase state
        {
            canSeePlayer = false;
            return chaseState;   
        }
        else if (staggered)//stagger
        {
            staggered = false;
            anim.SetTrigger("isHit");
            return staggerState;
        }
        else//idle state
        { 
            IdleMovement();
            return this;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canSeePlayer = true;
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
            navMeshAgent.SetDestination(destPoint);
            anim.SetBool("isWalking", true);
        }
        if (Vector3.Distance(this.transform.position, destPoint) < 1)
        {
            walkPointSet = false;
            canWalk = false;
            anim.SetBool("isWalking", false);
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

    public void IdleZone()
    {
        idleZone = this.transform.position;
    }

}
