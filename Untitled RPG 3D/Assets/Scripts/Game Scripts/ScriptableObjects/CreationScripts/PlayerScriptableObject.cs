using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public float speed;
    public float rollSpeed;
    public float rollTime;
    public float dashSpeed;
    public float dashTime;
}
