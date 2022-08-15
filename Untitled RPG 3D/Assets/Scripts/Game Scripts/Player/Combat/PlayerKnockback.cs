using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS SCRIPT IS VERY BUGGY AND NEEDS FURTHER WORK

public class PlayerKnockback : MonoBehaviour
{
    float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    Vector3 playerY;

    private CharacterController character;
    Animator anim;

    //action checkers
    bool isRolling;
    bool isKnockdown;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        isRolling = PlayerController.isRolling;

        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, Vector3 lookAtEnemy, float force)
    {
        if (!isRolling && !isKnockdown)
        {
            anim.SetTrigger("Knockdown");
            transform.LookAt(lookAtEnemy);
            PlayerController.isKnockdown = true;
            dir.Normalize();
            playerY = transform.position;
            if (dir.y > playerY.y) dir.y = playerY.y;
            // the line below cause the player to go upwards, could maybe be used in the future for downward slam attacks
            //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
            impact += dir.normalized * force / mass;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            isKnockdown = true;
            
        }
        
    }

    //this wouldn't run at the end of the animation, had to create an animation event
    public void KnockdownAnimEvent()
    {
        PlayerController.isKnockdown = false;
        PlayerController.isAttacking = false;
        isKnockdown = false;
    }
}