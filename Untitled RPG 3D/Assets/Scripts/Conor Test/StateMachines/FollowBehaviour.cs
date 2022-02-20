using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{

    private Transform playerPos;
    private float enemySpeed;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpeed = animator.GetComponent<EnemyTest>().speed;
        

    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JAHEnemySword.isAttacking = false;
        EnemyTest Distance = animator.GetComponent<EnemyTest>();
        //Move the Player
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, playerPos.position, enemySpeed * Time.deltaTime);
        //Rotate Enemy towards player

        animator.GetComponent<SmoothLookAt>().StartRotating();

        if (Distance.distFromPlayer > 10)
        {
            animator.SetBool("isFollowing", false);

        }
        if (Distance.distFromPlayer <= 2)
        
        {
            animator.SetTrigger("isAttacking");
            
       
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
