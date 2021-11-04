using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public int health;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
                
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log("Bumfucked");
    }
}
