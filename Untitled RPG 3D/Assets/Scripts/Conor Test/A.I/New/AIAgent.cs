using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{

    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public NavMeshAgent navMeshAgent;
    public EnemyScriptableObject config;
    public Transform Player;
    public AIAttack aiAttack;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        aiAttack = GetComponent<AIAttack>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.RegisterState(new DeathState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
