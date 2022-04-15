using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConXP : MonoBehaviour
{

    int randomAmount;
    public GameObject ExpOrb;
    float radius = 4f;


    public void DropXP()
    {

        randomAmount = Random.Range(10, 20);

        for (int i = 0; i < randomAmount; i++)
        {

            GameObject orb = Instantiate(ExpOrb, Random.insideUnitSphere * radius + transform.position, Random.rotation); //XP Drop

        }
    }
}

