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

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");    

    }

    void IdleZone()
    {
        idleZone = this.transform.position;
    }

    public override void EnterState(AIStateManager state)
    {
        IdleZone();

        //calling this to allow for mutiple stagger hits
        if(controller.isHit == true)
        {
            stateManager.IsHit();
        }
    }

    public override void UpdateState(AIStateManager state)
    {
        //idle movement
        controller.agent.speed = 2;

        if (!walkPointSet)
        {
            StartCoroutine(SearchForLocation());

        }
        if (canWalk)
        {
            controller.agent.isStopped = false;
            controller.agent.SetDestination(destPoint);
            controller.anim.SetBool("isWalking", true);
        }
        if (Vector3.Distance(this.transform.position, destPoint) < 1)
        {
            walkPointSet = false;
            canWalk = false;
            controller.anim.SetBool("isWalking", false);
            controller.agent.velocity = Vector3.zero;
            controller.agent.isStopped = true;

        }


        //switch to chase state
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10 || stateManager.angry == true)
        {
            stateManager.state = AIStateEnum.CHASE;
            state.SwitchToTheNextState(state.ChaseState);
            stateManager.angry = true;
        }
    }

    public override void ExitState(AIStateManager state)
    {
        controller.anim.SetBool("isWalking", false);
    }


    /*public override AIState RunCurrentState()
    {
        if(stateManager.state == AIStateEnum.CHASE)//chase state
        {   
            controller.anim.SetBool("isWalking", false);
            return stateManager.chaseState;    
        }
        else//idle state
        {
            IdleMovement();
            ActivateChaseState();
            return this;
        }   
    }*/

    /*void ActivateChaseState(AIStateManager state) 
    {
        
    }

    //move AI in idle zone
    void IdleMovement()
    {
        
    }*/
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

    
}
