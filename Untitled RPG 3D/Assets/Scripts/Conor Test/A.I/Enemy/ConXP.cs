using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConXP : MonoBehaviour
{

    int randomAmount;
    public GameObject expOrb;
    float radius = 4f;

     public Transform player;
      bool isFollowing = false;

 void Update()
 {
      //  if (isFollowing)
        transform.position = Vector3.Lerp(transform.position, player.position, 1 * Time.deltaTime);
 }
  
// void OnTriggerEnter(Collider other)
 //{
//    if (other.transform.tag == "Player")
 //   isFollowing = true;
// }
 
 void OnCollisionEnter(Collision other)
 {
    if (other.transform.tag == "Player")
    Destroy(expOrb);
 }

    public void DropXP()
    {

        randomAmount = Random.Range(10, 20);

        for (int i = 0; i < randomAmount; i++)
        {

            GameObject orb = Instantiate(expOrb, Random.insideUnitSphere * radius + transform.position, Random.rotation); //XP Drop

        }
    }


}

