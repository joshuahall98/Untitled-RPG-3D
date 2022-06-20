using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicGrunt : BTAgent
{

    private Transform Player;
    private Transform Enemy;
    [SerializeField] private EnemyTest EnemyVar;



    // Start is called before the first frame update
    new void Start()
    {
        EnemyVar = EnemyVar.GetComponent<EnemyTest>();
        Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();


        //Leaf Nodes - The Basic Decision
        Leaf guardArea = new Leaf("GuardArea", GuardArea);

        Leaf guardStand = new Leaf("Stand Still", GuardStanding);
        Leaf guardPatrol = new Leaf("GuardPatrol around Player", GuardPatrol);

        //Selector to choose what action
        RSelector guardChoice = new RSelector("Select Guard Choice");
        Selector BattleChoice = new Selector("Select Battle Choice");


        //When Player is detected
        Sequence inBattle = new Sequence("PlayerDetected");
        Leaf canSee = new Leaf("Can see Player", CanSeePlayer);
        Leaf approachPlayer = new Leaf("Approach Player", ApproachPlayer);
        //Leaf hideFromEnemy = new Leaf("Hide from the Enemy", HideFromEnemy, 2);


        //Inverts CanSeePlayer
        Inverter cantSeePlayer = new Inverter("Can't see Player");
        cantSeePlayer.AddChild(canSee);

        //IdleChoice

        guardChoice.AddChild(guardStand);
        guardChoice.AddChild(guardPatrol);

        //Player detected 
        inBattle.AddChild(canSee);
        inBattle.AddChild(approachPlayer);
        //BT Condition 

        BehaviourTree seePlayer = new BehaviourTree();
        seePlayer.AddChild(cantSeePlayer);

        //Sequence Node -- The Main Action PassThrough BT and Nav agent
        DepSequence enemyGuard = new DepSequence("ENEMY", seePlayer, agent);

        enemyGuard.AddChild(guardArea);
        enemyGuard.AddChild(guardChoice);


        //Join the Trees
        Selector iAmTheEnemy = new Selector("I am the Enemy");
        iAmTheEnemy.AddChild(enemyGuard);
        iAmTheEnemy.AddChild(inBattle);

        tree.AddChild(iAmTheEnemy);

        //FallBack  
        Selector PlayerisDead = new Selector("Fallback to follow");
        PlayerisDead.AddChild(enemyGuard);
        PlayerisDead.AddChild(guardChoice);





        tree.PrintTree();

    }

    public Node.Status GuardArea()
    {

        return tree.status;
    }


    public Node.Status GuardPatrol()
    {

        return tree.status;

    }

    public Node.Status GuardStanding()
    {

        agent.velocity = Vector3.zero;
        return tree.status;

    }


    public Node.Status CanSeePlayer()
    {
        //Player, tag, distance, pov
        Debug.Log("I can see");
        //Explain why this doesn't work please
        return TargetExists(Player.transform.position, "Player", 10 ,90);
        

    }

    public Node.Status ApproachPlayer()
    {

        if (EnemyVar.distFromPlayer > EnemyVar.attackRadius)
        {
            EnemyVar.agent.SetDestination(Player.transform.position ) ;
        }
        if (EnemyVar.distFromPlayer < EnemyVar.attackRadius)
        {
            //animator.SetBool("isAttacking", true);

        }
        else if (EnemyVar.distFromPlayer > EnemyVar.stoppingDistance)
        {
          //  animator.SetBool("isFollowing", false);
            EnemyVar.agent.SetDestination(Enemy.transform.position);
        }
        return tree.status;

    }

}
