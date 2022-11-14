using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{

    enum Direction { Left, Right}

    [SerializeField] Direction direction;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (direction == Direction.Left)
            {
                other.GetComponent<PlayerController>().CameraLeft();
                direction = Direction.Right;
            }
            
            else if (direction == Direction.Right)
            {
                other.GetComponent<PlayerController>().CameraRight();
                direction = Direction.Left;
            }


        }
    }
}
