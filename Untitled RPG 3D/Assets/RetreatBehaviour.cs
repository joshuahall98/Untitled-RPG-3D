using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyTest Player = animator.GetComponent<EnemyTest>();
        EnemyTest Distance = animator.GetComponent<EnemyTest>();
        EnemyTest retreatDist = animator.GetComponent<EnemyTest>();
        EnemyTest Agent = animator.GetComponent<EnemyTest>();

        if (Distance.distFromPlayer < retreatDist.retreatDistance)
        {
            Vector3 distToPlayer = animator.transform.position - Player.Player.transform.position;
            Vector3 newPos = animator.transform.position + distToPlayer;

            Agent.agent.SetDestination(newPos);
            
            animator.SetBool("isRetreating", true);

        }
        else 

        {
            animator.SetBool("isRetreating", false); ;


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
