using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public State currentState;
    public CharacterStats currentTarget;


    public bool isPerformingAction;
    public float maximumAttackRange = 1.5f;

    public float rotationSpeed = 15;

    [Header("A.I Settings")]
    public float detectionRadius = 20;
    public float maxiumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float speed = 5f;

    public float currentRecoveryTime = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        navmeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }

    void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if(currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
       
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

 

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //replace red with whatever color you prefer
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
