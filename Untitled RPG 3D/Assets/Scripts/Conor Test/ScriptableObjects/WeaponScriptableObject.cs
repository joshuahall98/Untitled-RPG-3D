using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public int baseDmg;
    public int heavyDmg;
    public int dashSpeed;
    public int dashDistance;
    
}
