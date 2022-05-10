using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    GameObject[] EnemyAI;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAI = GameObject.FindGameObjectsWithTag("Enemy");
       
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject moving in EnemyAI)
        {
            if
         (moving != gameObject)
            {
                EnemyTest stopDist = GetComponent<EnemyTest>();
                float distance = Vector3.Distance(moving.transform.position, this.transform.position);
                if (distance <= stopDist.AISpace)
                {
                    Vector3 direction = transform.position - moving.transform.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
    }
}
