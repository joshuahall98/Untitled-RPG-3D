using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveBehaviour : BTAgent
{

    public GameObject Player;
    public GameObject Safe;
    private Transform Enemy;


    
    // Start is called before the first frame update
    new void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        base.Start();
      

        //Leaf Nodes - The Basic Decision
        Leaf followPlayer = new Leaf("Following the Player", FollowPlayer);

        Leaf idleStand = new Leaf("Stand Still", IdleStandMethod, 2);
        Leaf idleMove = new Leaf("Wander around Player", Wander, 1);

        //Selector to choose what action
        RSelector idleChoice = new RSelector("Select Idle Choice");
        Selector BattleChoice = new Selector("Select Battle Choice");


        //When Enemy is detected
        Sequence inBattle = new Sequence("EnemyDetected");
        Leaf canSee = new Leaf("Can see Enemy", CanSeeEnemy);
        Leaf runAway = new Leaf("Run away", RunAwayMethod);
        //Leaf hideFromEnemy = new Leaf("Hide from the Enemy", HideFromEnemy, 2);


        //Inverts CanSeeEnemy
        Inverter cantSeeEnemy = new Inverter("Can't see Enemy");
        cantSeeEnemy.AddChild(canSee);

        //IdleChoice

        idleChoice.AddChild(idleStand);
        idleChoice.AddChild(idleMove);

        /*  //Battle Choice
          BattleChoice.AddChild(runAway);
          //  BattleChoice.AddChild(hideFromEnemy);
  */
        inBattle.AddChild(canSee);
        inBattle.AddChild(runAway);
        //BT Condition 

        BehaviourTree seeEnemy = new BehaviourTree();
        seeEnemy.AddChild(cantSeeEnemy);

        //Sequence Node -- The Main Action PassThrough BT and Nav agent
        DepSequence toFollowThePlayer = new DepSequence("COMPANION", seeEnemy, agent);

        toFollowThePlayer.AddChild(followPlayer);
        toFollowThePlayer.AddChild(idleChoice);


        //Join the Trees
        Selector iAmCompanion = new Selector("I am the Companion");
        iAmCompanion.AddChild(toFollowThePlayer);
        iAmCompanion.AddChild(inBattle);

        tree.AddChild(iAmCompanion);

        //FallBack  
        Selector PlayerisDead = new Selector("Fallback to follow");
        PlayerisDead.AddChild(toFollowThePlayer);
        PlayerisDead.AddChild(idleChoice);





        tree.PrintTree();

    }

    public Node.Status FollowPlayer()
    {
        //Going to player 

        Node.Status s = GoToLocation(Player.transform.position);
        if (s == Node.Status.SUCCESS)
        {

            Debug.Log("I follow the player");

        }
        return s;
    }

    

    public Node.Status HideFromEnemy()
    {
        Debug.Log("I chose to Hide");
        return tree.status;
        /*Node.Status s = InBattle();
        if(s == Node.Status.FAILURE)
        {
            hideFromEnemy.priortity = 10;
        }
        else
        {
            hideFromEnemy.priortity = 1;
        }
        return s;*/
    }
    public Node.Status Wander()
    {
        Debug.Log("Wandering");
        return tree.status;

    }

    public Node.Status IdleStandMethod()
    {
        /*if (Player.activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(Player.transform.position);
        if(s == Node.Status.SUCCESS)
        {
            agent.velocity = Vector3.zero;
        }
        return GoToLocation(Player.transform.position);*/
        Debug.Log("Standing Still");
        return tree.status;

    }


    public Node.Status CanSeeEnemy()
    {
        //Enemy, tag, distance, pov
       
        return EnemyExists(Enemy.transform.position, "Enemy", 1,90);
        
    }

    public Node.Status RunAwayMethod()
    {
        //Run from Enemy, distance
        Debug.Log("Fleeing");
        return RunAway(Enemy.transform.position, 10);

    }

}
