using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public GameObject enemy;
public Transform EnemyPos;
private float repeatTimer = 2f;*/

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
/*
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 0.5f, repeatTimer);
            Destroy(gameObject, 11);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
  void EnemySpawner()
    {
        Instantiate(enemy, EnemyPos, EnemyPos.rotation);
    }*/
}
