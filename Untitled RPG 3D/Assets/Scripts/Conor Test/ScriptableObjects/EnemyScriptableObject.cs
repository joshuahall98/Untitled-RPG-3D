using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public int maxHP;
    public float speed;
    public float retreatDist;
    public float distFromAI;
    public float stoppingDist;


}
