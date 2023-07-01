using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoDistance : MonoBehaviour
{
    [SerializeField] float size;

    GameObject player;

    [SerializeField]float distance;

    [SerializeField]float divideAmount;

    Vector3 compareScale;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        compareScale = this.transform.localScale;
        size = Camera.main.orthographicSize;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);

        if(distance > 1) 
        {
            Vector3 scale = new Vector3(size / (distance / divideAmount), size / (distance / divideAmount), size / (distance / divideAmount));

            if (scale.x > compareScale.x && scale.y > compareScale.y  && scale.z > compareScale.z)
            {
                this.transform.localScale = compareScale;
            }
            else
            {
                this .transform.localScale = scale; 
            }

            

            
        }
        
    }
}
