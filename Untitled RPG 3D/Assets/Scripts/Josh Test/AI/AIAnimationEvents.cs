using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationEvents : MonoBehaviour
{

    [SerializeField]AIStateManager stateManager;


    public void AEReturnToIdle()
    {
        Debug.Log("Idle");
        stateManager.state = AIStateEnum.IDLE;
    }

}
