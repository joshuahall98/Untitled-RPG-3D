using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    Vector3 playerY;

    private CharacterController character;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerKnockdown"))
        {
            PlayerController.isAttacking = true;
        }
        else
        {
            PlayerController.isAttacking = false;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, Vector3 lookAtEnemy, float force)
    {
        anim.SetTrigger("Knockdown");
        transform.LookAt(lookAtEnemy);
        //PlayerController.isAttacking = true;
        dir.Normalize();
        playerY = transform.position;
        if (dir.y > playerY.y) dir.y = playerY.y;
        // the line below cause the player to go upwards, could maybe be used in the future for downward slam attacks
        //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    //this wouldn't run at the end of the animation, had to create an animation event
    /*public void KnockdownAnimEvent()
    {
        PlayerController.isAttacking = false;
    }*/
}
