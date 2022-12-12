using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chasePlayerstate;
    public LayerMask dectectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, dectectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                //Having Player + Enemy classes dervice from Character stats makes it easy to check if the object has the script.
                //Will allow us to have factions attack each other down the line
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    //Check for team ID i.e Friend or Foe

                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maxiumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }

        
        if(enemyManager.currentTarget != null)
        {
            return chasePlayerstate;
        }
        else
        {
            return this;
        }
    }
}

