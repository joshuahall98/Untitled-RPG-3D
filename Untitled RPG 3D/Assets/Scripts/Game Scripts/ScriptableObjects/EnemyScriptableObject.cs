using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public float maxHP;
    public float speed;
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxSightDistance = 5f;
    public float attackRadius;
    public int damage;
    public float AvoidancePredictionTime = 2;
    public int PathfindingIterationsPerFrame = 100;
    public float knockBackForce;

}