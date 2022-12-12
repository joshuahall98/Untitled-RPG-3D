using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AttackState : State
{

    public CombatStanceState combatStanceState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPerformingAction)
            return combatStanceState;

        if(currentAttack != null)
        {
            //If enemy is too close to attack, get another attack
            if(enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            else if(enemyManager.distanceFromTarget < currentAttack.maxiumDistanceNeededToAttack)
            {
                if(enemyManager.viewableAngle <= currentAttack.maxiumAttackAngle && enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if(enemyManager.currentRecoveryTime <=0 && enemyManager.isPerformingAction == false)
                    {
                        enemyAnimatorManager.anim.SetFloat("Vertical", 0, 01f, Time.deltaTime);
                        enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
                }
            }

        }
        else
        {
            GetNewAttack(enemyManager);
        }
        return combatStanceState;
    }
        private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (enemyManager.distanceFromTarget <= enemyAttackAction.maxiumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxiumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (enemyManager.distanceFromTarget <= enemyAttackAction.maxiumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxiumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
