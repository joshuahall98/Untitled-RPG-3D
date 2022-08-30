using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject door;

    bool move = false;
    [SerializeField] bool stopPlayer;
    [SerializeField] bool stopRewind;

    [SerializeField] float speed;
    [SerializeField] float distance;
    Vector3 target;

    GameObject player;

    enum DoorType { DoorDown, DoorUp, DoorReturnUpOrDown}

    [SerializeField] DoorType doorType;

    private void Start()
    {
        player = GameObject.Find("Player");

        //check the door type to set the position it needs to move to
        if(doorType == DoorType.DoorDown)
        {
            target = new Vector3(door.transform.position.x, door.transform.position.y - distance, door.transform.position.z);
        }
        else if (doorType == DoorType.DoorUp)
        {
            target = new Vector3(door.transform.position.x, door.transform.position.y + distance, door.transform.position.z);
        }
        else if(doorType == DoorType.DoorReturnUpOrDown)
        {
            target = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //move the doors depending on chosen enum
        if ((move == true && doorType == DoorType.DoorDown) || (move == true && doorType == DoorType.DoorUp) || (move == true && doorType == DoorType.DoorReturnUpOrDown))
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            door.transform.position = Vector3.MoveTowards(door.transform.position, target, step);

            StopPlayer();
            
        }

        if (door.transform.position == target && move == true)
        {
            PlayerController.isAttacking = false;
            move = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            move = true;
            //better to remove collider instead of destroy when player can still move after activating a door
            if (stopPlayer == false)
            {
                GetComponent<Collider>().enabled = false;

            }

            if(stopRewind == true)
            {
                StartCoroutine(StopRewind());
            }
        }
        
    }

    void StopPlayer()
    {
        if (stopPlayer == true)
        {
            PlayerController.isAttacking = true;
            stopPlayer = false;
        }
    }

    IEnumerator StopRewind()
    {
        player.GetComponent<PlayerController>().DisableRewind();

        yield return new WaitForSeconds(5.5f);

        player.GetComponent<PlayerController>().EnableRewind();
    }
}
