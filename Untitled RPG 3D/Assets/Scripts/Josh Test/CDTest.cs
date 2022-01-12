using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDTest : MonoBehaviour
{
    [SerializeField]float rollCDTimer = 0;
    [SerializeField]int dashUsed = 0;

    // Update is called once per frame
    void Update()
    {
        
        if(rollCDTimer > 0)
        {
            rollCDTimer -= Time.deltaTime;
            
        }
        else if ((rollCDTimer > 0) && (dashUsed == 3))
        {
            //disable roll
            //cause mental issues
        }
        else
        {
            dashUsed = 0;
            //enable dash
            //return to beaver
        }

    }

    void Dash()
    {
        dashUsed = dashUsed + 1;
        rollCDTimer = 5;
    }
}
