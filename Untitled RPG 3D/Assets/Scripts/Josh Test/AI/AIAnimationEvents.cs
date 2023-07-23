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

}
