using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationEvents : MonoBehaviour
{

    [SerializeField] AIAttackState attackState;
    [SerializeField] AIStaggerState staggerState;

    public void AEStaggerFin()
    {
        staggerState.StaggerFin();
    }

    public void AEAttackFin()
    {
        attackState.AttackFin();
    }
}
