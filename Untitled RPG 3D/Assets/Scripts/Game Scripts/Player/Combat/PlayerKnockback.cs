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
    PlayerAnimController anim;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<PlayerAnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        //this line seems to stop the player being knockedback into oblivion
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        if(anim.IsAnimationDone(PlayerAnimController.PlayerAnimState.Knockdown) && PlayerController.state == PlayerState.KNOCKEDDOWN)
        {
            KnockdownEndAnim();
        }
         
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, Vector3 lookAtEnemy, float force)
    {

        if (PlayerController.state == PlayerState.IDLE || PlayerController.state == PlayerState.MOVING || PlayerController.state == PlayerState.ATTACKING || PlayerController.affect == PlayerAffect.DIZZY  || PlayerController.state == PlayerState.INTERACTING)
        {
            if(PlayerController.immune == false)
            {
               // GetComponent<PlayerController>().canMove = false;

                //cancel interact
                if (PlayerController.state == PlayerState.INTERACTING)
                {
                    GetComponent<PlayerController>().Interact();
                }

                //run death if health = 0
                GetComponent<PlayerHealth>().CheckIfDead();

                PlayerController.state = PlayerState.KNOCKEDDOWN;

                anim.ChangeAnimationState(PlayerAnimController.PlayerAnimState.Knockdown, 0.1f, 0);
               // anim.SetTrigger("Knockdown");
                transform.LookAt(lookAtEnemy);
                GetComponent<PlayerController>().DisableRewind(); //this prevents rewind bug
                dir = dir.normalized;

                //THIS WAS THE OLD KNOCKBACK CODE, CAUSED THE PLAYER TO GO UP TOO
                //dir.Normalize();
                playerY = transform.position;
                if (dir.y > playerY.y) dir.y = playerY.y;
                // the line below cause the player to go upwards, could maybe be used in the future for downward slam attacks
                //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
                //impact += dir.normalized * force / mass;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                impact = dir * force;
            }     
        }
    }

    //this wouldn't run at the end of the animation, had to create an animation event
    public void KnockdownEndAnim()
    {
        GetComponent<PlayerController>().EnableRewind();

     //   GetComponent<PlayerController>().canMove = true;
        PlayerController.state = PlayerState.IDLE;

        //StartCoroutine(ActionDelay());
    }

    //work around to prevent buggy interaction when the character uses actions after being knockdown
    /*IEnumerator ActionDelay()
    {
        GetComponent<PlayerController>().DisableRoll();
        GetComponent<PlayerController>().DisableLightAttack();
        GetComponent<PlayerController>().DisableHeavyAttackCharge();

        yield return new WaitForSeconds(0.3f);

        GetComponent<PlayerController>().EnableRoll();
        GetComponent<PlayerController>().EnableLightAttack();
        GetComponent<PlayerController>().EnableHeavyAttackCharge();


    }*/

}
