using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationEvents : MonoBehaviour
{

    [SerializeField]AIStateManager stateManager;


    public void AEReturnToIdle()
    {
        stateManager.state = AIStateEnum.IDLE;
    }

    public void AEWeaponColliderOn()
    {
        this.GetComponent<WeaponColliders>().WeaponColliderOn();
    }

    public void AEWeaponColliderOff()
    {
        this.GetComponent<WeaponColliders>().WeaponColliderOff();
    }

    public void AECheckIfDead()
    {
        this.GetComponent<AIHealth>().CheckIfDead();
    }

}
