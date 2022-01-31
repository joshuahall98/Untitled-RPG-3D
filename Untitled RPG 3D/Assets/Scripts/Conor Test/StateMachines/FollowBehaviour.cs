using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{

    private Transform playerPos;
    public float speed;
    float distFromPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distFromPlayer = Vector3.Distance(playerPos.position, animator.transform.position);
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, playerPos.position, speed * Time.deltaTime);
        if(distFromPlayer > 15)
        {
            animator.SetBool("isFollowing", false);
            JAHEnemySword.isAttacking = false;

        }
        if (distFromPlayer <= 2)
        {
            animator.SetTrigger("isAttacking");

            //this is meant to enable the hit box for the sword while you're attacking, doesn't work though, it is somewhat working as it does log the attacking temporarily then stops
            //how to keep hit box active while attacking??
            JAHEnemySword.isAttacking = true;
            Debug.Log("I am attacking");
        }
        else
        {
          
        }

    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
