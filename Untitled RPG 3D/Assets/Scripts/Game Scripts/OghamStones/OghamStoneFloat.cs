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
    public bool inRange = false;

    Vector3 startingPos;
    [SerializeField]Vector3 rotation;
    Vector3 originalPos;
    Quaternion originalRotation;
    [SerializeField]float vecCounter;
    [SerializeField] float vecCounterCompare;
    public float distance;

    


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRotation = transform.rotation;
        currentPosY = transform.position.y;
        height = Random.Range(0.9f, 1.1f);
        distance = Random.Range(0.1f, 0.3f);
        floatSpeed = Random.Range(0.9f, 1.1f);
        newPosY = currentPosY + height;

        rotation = new Vector3(0, 0, Random.Range(-15, 15));

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        inRange = OghamStoneRadius.inRange;

        //getting the objects to float up
        if(rotated == false && inRange == true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, newPosY, transform.position.z), moveSpeed * Time.deltaTime);
            startingPos = transform.position;
            
        }
        
        //stop the objects floating up when it gets over a certain point
        if (transform.rotation.eulerAngles.x >= 80)
        {
            rotated = true;
        }

        if(rotated == true)
        {
            transform.Rotate(rotation * Time.deltaTime);


            //this small snippet below caluclates the sine waves position and commeneces the floating of the objects when the sine wave matches their current position, this prevents stutter
            vecCounter = Mathf.Sin(Time.time * floatSpeed) * distance + startingPos.y;
            vecCounter = Mathf.Round(vecCounter * 100.0f) * 0.01f;
  
            vecCounterCompare = Mathf.Round((startingPos.y) * 100.0f) * 0.01f;

            if (vecCounter == vecCounterCompare)
            {
                startFloating = true;
            }

            //getting the objects to float up and down following the sine wave
            if (startFloating == true)
            {
                transform.position = new Vector3(startingPos.x, Mathf.Sin(Time.time * floatSpeed) * distance + startingPos.y, startingPos.z);
                
            }
        }

        if(inRange == false)
        {
            StonesInactive();
        }
    }

    void StonesInactive()
    {
        rotated = false;
        startFloating = false;
        transform.position = Vector3.MoveTowards(transform.position, originalPos, moveSpeed * Time.deltaTime); ;
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, turnSpeed * Time.deltaTime); 
    }


}
