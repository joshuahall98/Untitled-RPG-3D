using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction)
        {
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
        }


        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        Vector3 fleeFromTarget = enemyManager.transform.position + transform.forward * 5f;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (distanceFromTarget < enemyManager.maximumAttackRange)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            //Look Away from player
            transform.rotation = Quaternion.LookRotation(transform.position - enemyManager.currentTarget.transform.position);

            //Extra Code
            targetDirection.Normalize();
            targetDirection.y = 0;

            enemyManager.speed = 10;
            targetDirection *= enemyManager.speed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            enemyManager.enemyRigidBody.velocity = projectedVelocity;
            enemyManager.navmeshAgent.SetDestination(fleeFromTarget);
            //End
        }

        enemyManager.navmeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navmeshAgent.transform.localRotation = Quaternion.identity;

        if (distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }

    }

}

