using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorFollow : MonoBehaviour
{

    GameObject player; 

    private void Start()
    {
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - 1.2f, player.transform.position.z);



    }
}
