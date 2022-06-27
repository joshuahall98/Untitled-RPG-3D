using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementBehaviour : StateMachineBehaviour
{
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyTest enemyTest = animator.GetComponent<EnemyTest>();


        //go to the Player
        Vector3 newPos = enemyTest.Player.transform.position;


        if (enemyTest.distFromPlayer > enemyTest.attackRadius)
        {
            enemyTest.agent.SetDestination(newPos);
        }
        if (enemyTest.distFromPlayer < enemyTest.attackRadius)
        {
            animator.SetBool("isCharging", true);

        }
        else if (enemyTest.distFromPlayer > enemyTest.stoppingDistance)
        {
            animator.SetBool("isFollowing", false);
            enemyTest.agent.SetDestination(animator.transform.position);
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
