using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public float maxHP; //max health
    public float speed; //idle speed
    public int damage; // base attack damage
    public float sightDistance; //distance enemy can detect player
    public float knockBackForce; //force applied when hitting player
    public float attackRange; // attack range

    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxSightDistance = 5f;
    
    
    public float AvoidancePredictionTime = 2;
    public int PathfindingIterationsPerFrame = 100;
   

}
