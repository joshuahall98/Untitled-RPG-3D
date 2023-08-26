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
    

    public override void EnterState(AIStateManager state)
    {

        //string nextState = AIController.AnimState.Chase.ToString();
        //Debug.Log(nextState);
       // controller.ChangeAnimationState(nextState);
        controller.ChangeAnimationState(AIController.AnimState.Idle, 0.2f, 0);

        idleZone = this.transform.position;//create the idle zone starting point

        //calling this to allow for mutiple stagger hits
        if (controller.isHit == true)
        {
            stateManager.IsHit();
        }
        else if (stateManager.GetComponent<AIHealth>().currentHealth < controller.stats.maxHP / 2)
        {
            state.SwitchToTheNextState(state.FleeState);
            
        }
    }

    public override void UpdateState(AIStateManager state)
    {
        IdleMovement();

        SwitchToChase(state);
        
    }

    public override void ExitState(AIStateManager state)
    {
       // controller.anim.SetBool("isWalking", false);
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

    private void IdleMovement()
    {
        //idle movement
        controller.agent.speed = controller.stats.speed / 2;

        if (!walkPointSet)
        {
            StartCoroutine(SearchForLocation());

        }
        if (canWalk)
        {
            controller.agent.isStopped = false;
            controller.agent.SetDestination(destPoint);
            controller.ChangeAnimationState(AIController.AnimState.IdleWalk, 0.1f, 0);
            //controller.anim.SetBool("isWalking", true);
        }
        if (Vector3.Distance(this.transform.position, destPoint) < 1)
        {
            walkPointSet = false;
            canWalk = false;
            controller.ChangeAnimationState(AIController.AnimState.Idle, 0.2f, 0);
            //controller.anim.SetBool("isWalking", false);
            controller.agent.velocity = Vector3.zero;
            controller.agent.isStopped = true;

        }
    }

    private void SwitchToChase(AIStateManager state)
    {
        //when the player returns to idle after attacking, they need to first check to make sure the player is still within chasing range, if not stay in idle
        if(Vector3.Distance(this.transform.position, controller.player.transform.position) > controller.stats.deagroRange)
        {
            stateManager.angry = false;
        }
        else if(Vector3.Distance(this.transform.position, controller.player.transform.position) < controller.stats.agroRange || stateManager.angry == true)
        {
            state.SwitchToTheNextState(state.ChaseState);
            stateManager.angry = true;
        }

    }

}
