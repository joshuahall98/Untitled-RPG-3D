using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject gate;

    [SerializeField]bool move = false;

    [SerializeField] float speed;
    Vector3 target;

    private void Start()
    {
        target = new Vector3(gate.transform.position.x, gate.transform.position.y - 9, gate.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (move == true)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            gate.transform.position = Vector3.MoveTowards(gate.transform.position, target, step);

            PlayerController.isAttacking = true;
        }

        if(gate.transform.position == target)
        {
            PlayerController.isAttacking = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        move = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(this.gameObject);
    }
}
