using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOghamStone : MonoBehaviour
{
    private Vector3 lastPos, currPos;
    private float rotationSpeed = -0.2f;
    float rotation;
    [SerializeField]float distance;
    [SerializeField]float distance2;
    float floatSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = Input.mousePosition;
        rotation = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotation, transform.eulerAngles.z);
        distance = Random.Range(0.01f, 0.03f);
        floatSpeed = Random.Range(0.9f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * floatSpeed) * distance + transform.position.y, transform.position.z);

        if (Input.GetMouseButton(0))
        {
            currPos = Input.mousePosition;
            Vector3 offset = currPos - lastPos;
            transform.RotateAround(transform.position, Vector3.up, offset.x * rotationSpeed);
            lastPos = currPos;
        }
            lastPos = Input.mousePosition;
        
    }

}
