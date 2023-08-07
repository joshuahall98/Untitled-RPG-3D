using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;
using UnityEngine.AI;

public class JAHKnockback : MonoBehaviour
{
    [SerializeField]float knockbackStrength;
    [SerializeField]float tempStrength;
    [SerializeField]GameObject player;

    Vector3 playerPos;

    [SerializeField]Rigidbody rb;

    GameObject hitObject;
    Vector3 direction;

    public bool collisionHappened;

    private void OnTriggerEnter(Collider collision)
    {

        rb = collision.GetComponent<Rigidbody>();

        if (rb != null)
        {

            direction = collision.transform.position - player.transform.position;
            direction.y = 0;

            hitObject = collision.gameObject;
            playerPos = player.transform.forward;

            hitObject.GetComponent<AIStateManager>().IsHit();
            hitObject.GetComponent<AIController>().KnockedBack(knockbackStrength, playerPos);

        }
    }

}
