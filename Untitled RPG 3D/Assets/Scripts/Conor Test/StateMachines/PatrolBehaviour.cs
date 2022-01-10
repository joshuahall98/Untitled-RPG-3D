using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    public Transform Player;
    private PatrolArea wayPoints;
    public float speed;
    private int randomWayPoint;
    public float distFromPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wayPoints = GameObject.FindGameObjectWithTag("WayPoint").GetComponent<PatrolArea>();
        randomWayPoint = Random.Range(0, wayPoints.waypoints.Length);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, wayPoints.waypoints[randomWayPoint].position) < 0.2f){

            animator.transform.position = Vector3.MoveTowards(animator.transform.position, wayPoints.waypoints[randomWayPoint].position, speed * Time.deltaTime);
        }
        else
        {
            randomWayPoint = Random.Range(0, wayPoints.waypoints.Length);
        }
        if (distFromPlayer <= 5)
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
