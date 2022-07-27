using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OghamStoneFloat : MonoBehaviour
{
    public float turnSpeed = 2;
    public float floatSpeed = 1;
    public float moveSpeed = 2;

    [SerializeField] float currentPosY;
    [SerializeField] float newPosY;

    [SerializeField]float height;

    public bool rotated = false;
    public bool startFloating = false;

    Vector3 startingPos;
    [SerializeField]Vector3 rotation;
    [SerializeField]float vecCounter;
    public float distance;

    [SerializeField]float maxHeight;

    float midPoint;
    [SerializeField]float myvar;


    // Start is called before the first frame update
    void Start()
    {

        currentPosY = transform.position.y;
        height = Random.Range(0.9f, 1.1f);
        distance = Random.Range(0.1f, 0.3f);
        floatSpeed = Random.Range(0.9f, 1.1f);
        newPosY = currentPosY + height;

        rotation = new Vector3(0, 0, Random.Range(-15, 15));

    }

    // Update is called once per frame
    void Update()
    {


        if(rotated == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, newPosY, transform.position.z), moveSpeed * Time.deltaTime);
            startingPos = transform.position;
            
        }
        
        if (transform.rotation.eulerAngles.x >= 80)
        {
            rotated = true;
        }

        if(rotated == true )
        {
            transform.Rotate(rotation * Time.deltaTime);

            vecCounter = Mathf.Sin(Time.time * floatSpeed) * distance + startingPos.y;
            vecCounter = Mathf.Round(vecCounter * 100.0f) * 0.01f;
  
            myvar = Mathf.Round((startingPos.y) * 100.0f) * 0.01f;

            if (vecCounter == myvar)
            {
                startFloating = true;
            }


            if (startFloating == true)
            {
                transform.position = new Vector3(startingPos.x, Mathf.Sin(Time.time * floatSpeed) * distance + startingPos.y, startingPos.z);
                
            }


        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //rocks go float
        //if(collsion == player){
        //
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //rocks stop float
    }

}
