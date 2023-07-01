using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIChaseState : AIState
{
    public AIAttackState attackState;
    public AIIdle idleState;

    public bool isInAttackRange;
    public bool isOutOfRange;

    NavMeshAgent navMeshAgent;

    GameObject player;

    Animator anim;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponentInParent<Animator>();
        player = GameObject.Find("Player");
    }

    public override AIState RunCurrentState()
    {
        if (isInAttackRange)//attack state
        {
            isInAttackRange = false;
            return attackState;
        }
        else if (isOutOfRange)//idle state  THIS SHOULD ALSO GO TO PATROL WHERE APPLICABLE OR STAND STILL
        {
            OutOfRange();
            return idleState;
        }
        else//chase state
        {
            ChasePlayer();
            return this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            isOutOfRange = true;
        }
    }

    void ChasePlayer()
    {
        anim.SetBool("isChasing", true);
        navMeshAgent.speed = 4;
        this.navMeshAgent.SetDestination(player.transform.position);
        if (Vector3.Distance(this.transform.position, player.transform.position) < 2)
        {
           // isInAttackRange = true;
        }
    }

    void OutOfRange()
    {
        anim.SetBool("isChasing", false);
        this.navMeshAgent.SetDestination(this.transform.position);
        isOutOfRange = false;
        //idleState.IdleZone();//reset the idle zone to where the AI has stopped
    }

}
  
