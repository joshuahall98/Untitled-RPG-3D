using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : StateMachineBehaviour
{
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    private float enemySpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform wayPointsObj = GameObject.FindGameObjectWithTag("Waypoints").transform;
       
        foreach (Transform W in wayPointsObj)
        {
            wayPoints.Add(W);
        }
        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[0].position);
        
        enemySpeed = animator.GetComponent<EnemyTest>().speed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
        EnemyTest Distance = animator.GetComponent<EnemyTest>();

        //Move the Player

       if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

           
        }    
            
       
        if (Distance.distFromPlayer <15)
        {
            animator.SetBool("isPatrolling", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
