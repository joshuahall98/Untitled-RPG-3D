using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public ChaseState chaseState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if(enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maxiumAttackRange)
        {
            return attackState;
        }
        else if(enemyManager.distanceFromTarget > enemyManager.maxiumAttackRange)
        {
            return chaseState;
        }
        else
        {
            return this;
        }

    }
   
 
}
