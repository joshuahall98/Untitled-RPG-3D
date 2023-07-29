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
    float weight;

    Vector3 playerPos;
    Vector3 hitObjectPos;

    [SerializeField]Rigidbody rb;

    GameObject hitObject;
    Vector3 direction;

    [SerializeField]bool isHit;

    private void FixedUpdate()
    {
        if (isHit)
        {
            //hitObject.transform.Translate(direction * knockbackStrength * Time.deltaTime);
            hitObject.transform.position += playerPos * Time.deltaTime * (tempStrength -= weight);
            if(tempStrength <= 0)
            {
                tempStrength = 0;
                isHit = false;
            }
        }
        

    }

    private void OnTriggerEnter(Collider collision)
    {
        

        rb = collision.GetComponent<Rigidbody>();


        if (rb != null)
        {
            //StartCoroutine(StopMovement());
            //rb.isKinematic = false;
     
            direction = collision.transform.position - player.transform.position;
            direction.y = 0;
            
            //rb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
            //rb.velocity = direction * knockbackStrength;

            hitObject = collision.gameObject;

            /*hitObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            hitObject.GetComponent<NavMeshAgent>().isStopped = true;*/

            playerPos = player.transform.forward;
            hitObjectPos = hitObject.transform.position;

            weight = knockbackStrength / 25;
            tempStrength = knockbackStrength;
            isHit = true;

            hitObject.GetComponent<Animator>().SetTrigger("isHit");

            Debug.Log("Hit");
            
        }


    }

    /*IEnumerator StopMovement()
    {

        yield return new WaitForSeconds(.2f);

        Debug.Log("Stop");

       // rb.velocity = Vector3.zero;

        isHit = false;

        // hitObject.GetComponent<NavMeshAgent>().isStopped = false;
    }*/

}
