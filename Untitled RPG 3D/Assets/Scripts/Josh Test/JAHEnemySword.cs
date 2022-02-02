using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHEnemySword : MonoBehaviour
{
    public static bool isAttacking = false;

    public GameObject swordHitBox;

    private void Update()
    {
        
        if (isAttacking == true)
        {
            swordHitBox.SetActive(true);
                      
        }
        else
        {
            swordHitBox.SetActive(false);
        }
    }

}
