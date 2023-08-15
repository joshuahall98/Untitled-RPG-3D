using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class AIController : MonoBehaviour
{

    public Animator anim;
    public NavMeshAgent agent;
    public EnemyScriptableObject stats;
    public GameObject player;

    float tempStrength;
    public bool isHit;
    float weight;
    Vector3 playerPos;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHit)
        {
            //hitObject.transform.Translate(direction * knockbackStrength * Time.deltaTime);
            this.transform.position += playerPos * Time.deltaTime * (tempStrength -= weight);
            if (tempStrength <= 0)
            {
                tempStrength = 0;
                isHit = false;
            }
        }
    }

    public void KnockedBack(float knockbackStrength, Vector3 playerPosition)
    {
        tempStrength = 0;
        tempStrength = knockbackStrength;
        isHit = false;
        isHit = true;
        weight = knockbackStrength / 25;
        playerPos = playerPosition;
        anim.SetTrigger("Hit");//this has to be called here for looping animation

    }

    public void RotateToPlayer()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100);
    }


}
