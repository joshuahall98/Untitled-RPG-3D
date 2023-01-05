using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{

    NavMeshAgent agent;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (agent.hasPath)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    public void ResetAttackTrigger()
    {
        animator.ResetTrigger("isAttacking");
        agent.isStopped = false;

    }

}
