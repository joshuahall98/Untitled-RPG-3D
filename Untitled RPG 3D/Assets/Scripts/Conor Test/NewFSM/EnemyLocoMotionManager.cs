using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocoMotionManager : MonoBehaviour
{
    EnemyManager  enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
 

    
   


    private void Awake()
    {
        
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
       
    }

  

   
    }


