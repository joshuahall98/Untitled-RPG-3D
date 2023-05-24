using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Conor 
    public class AIAgent : MonoBehaviour
    {

        public AIStateMachine stateMachine;
        public AIStateID initialState;
        public NavMeshAgent navMeshAgent;
        public EnemyScriptableObject config;
        public Transform Player;
        public AIAttack aiAttack;
        public Animator animator;
        public AIHealth aiHealth;


        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            aiAttack = GetComponent<AIAttack>();
            aiHealth = GetComponent<AIHealth>();

            //Scriptable Object

            //Navmesh Avoidance
            NavMesh.avoidancePredictionTime = config.AvoidancePredictionTime;
            NavMesh.pathfindingIterationsPerFrame = config.PathfindingIterationsPerFrame;

            //Changing NavMesh Properties
            navMeshAgent.speed = config.speed;
            Debug.Log("Speed is" + config.speed);

            //For Every new State we have to Register it here

            stateMachine = new AIStateMachine(this);
            stateMachine.RegisterState(new AIIdleState());
            stateMachine.RegisterState(new AIChasePlayerState());
            stateMachine.RegisterState(new AiAttackPlayerState());
            stateMachine.RegisterState(new CelebrationState());
            stateMachine.RegisterState(new DeathState());
            stateMachine.RegisterState(new FleeState());
            stateMachine.RegisterState(new PatrolState());
            
            

            stateMachine.ChangeState(initialState);


        }

        // Update is called once per frame
        void Update()
        {

            stateMachine.Update();
        }

    }
