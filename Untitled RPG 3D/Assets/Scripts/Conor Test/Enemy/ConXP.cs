using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConXP : MonoBehaviour
{
    
    int randomAmount;
    public GameObject ExpOrb;

    

    public void DropXP()
    {
        randomAmount = Random.Range(1, 4);
       
        for (int i = 0; i < randomAmount; i++) {

            Vector3 position = transform.position;
            GameObject orb = Instantiate(ExpOrb, position, Quaternion.identity); //XP Drop
            
        }
    }
}

