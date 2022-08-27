using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
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
        //this is meant to enable the hit box for the sword while you're attacking, doesn't work though, it is somewhat working as it does log the attacking temporarily then stops
        //how to keep hit box active while attacking??

        //JAHEnemySword.isAttacking = true;

        //!!!!!!!!!!!!!Hey ConCon, the above line is uneccesary now, I have hitbox activating using animation events as it's smoother, fixes bug!!!!!!!!!!!!!!!!!!!!!!!!!!!
        EnemyTest Agent = animator.GetComponent<EnemyTest>();
        Agent.agent.velocity = Vector3.zero;

        animator.SetBool("isFollowing", true);
        animator.SetBool("isCharging", false);

        // animator.transform.position = Vector3.MoveTowards(animator.transform.position, playerPos.position, -enemySpeed * Time.deltaTime);


    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {

        animator.ResetTrigger("Attack");
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
