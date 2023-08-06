using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    public Animator anim;
    public NavMeshAgent agent;

    float tempStrength;
    public bool isHit;
    float weight;
    Vector3 playerPos;

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

    }
}
