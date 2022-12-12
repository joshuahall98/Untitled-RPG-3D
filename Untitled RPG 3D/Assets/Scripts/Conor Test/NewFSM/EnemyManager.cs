using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    EnemyLocoMotionManager enemyLocoMotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public State currentState;
    public  bool isPerformingAction;
    public CharacterStats currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;


    public float distanceFromTarget;
    public float rotationSpeed = 15;
    public float maxiumAttackRange = 1.5f;



    [Header("AI Settings")]
    public float detectionRadius = 20;

    //The higher, and lower, the greater detection FOV;
    public float maxiumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;

    public float currentRecoveryTime = 0;


    // Start is called before the first frame update
    void Awake()
    {
        enemyLocoMotionManager = GetComponent<EnemyLocoMotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        navmeshAgent.enabled = false;
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {

        enemyRigidBody.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRecoveryTimer();
    }

    public void FixedUpdate()
    {
        HandleStateMachine();

    }

    private void HandleStateMachine()
    {
        if(currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if(nextState != null)
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
  

        
    //View Aggro Box
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
