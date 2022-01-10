using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea : MonoBehaviour
{
    public float speed;
    private float waitTime;
    public float startWaitTime;

    public Transform[] waypoints;
    private int randomWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        randomWayPoint = Random.Range(0, waypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[randomWayPoint].position, speed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position, waypoints[randomWayPoint].position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                randomWayPoint = Random.Range(0, waypoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
