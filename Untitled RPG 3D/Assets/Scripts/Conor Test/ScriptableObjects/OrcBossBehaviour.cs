using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcBossBehaviour : StateMachineBehaviour
{

    private int randomChoice;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomChoice = Random.Range(0, 2);
        
        if(randomChoice == 0)
        {
            animator.SetTrigger("Attk1");
        }
        else
        {
            animator.SetTrigger("Attk2");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("PlayerInRange", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }


}
