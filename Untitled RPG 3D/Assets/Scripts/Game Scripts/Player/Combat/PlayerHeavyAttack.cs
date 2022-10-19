using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : MonoBehaviour
{
    //Animation
    Animator anim;

    //action checkers
    public bool isMoving;
    public bool isRolling;
    public bool isAttacking;
    public bool isDizzy;
    public bool isGrounded;
    public bool isDead;
    public bool isKnockdown;

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    public GameObject sparkle;
    Collider swordCollider;

    public bool releaseReady = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        swordCollider = sword.GetComponent<Collider>();

        sparkle.SetActive(false);
    }


    public void HeavyAtkCharge()
    {
        CheckActions();

        if (!isRolling)
        {
            if (!isDizzy)
            {
                if (isGrounded)
                {
                    if (!isKnockdown)
                    {
                        if (!isAttacking)
                        {
                            anim.SetTrigger("HeavyAttackHold");
                            anim.ResetTrigger("HeavyAttackFail");
                            PlayerController.isAttacking = true;
                            sword.SetActive(true);
                            sheathedSword.SetActive(false);
                            swordCollider.enabled = false;
                            releaseReady = false;
                            GetComponent<SoundManager>().PlaySound("Heavy Attack Charge");
                        }
                    }
                    
                }
            }
        }


    }

    //this line of code checks to see if the animation has reached full charge before allowing the swing to commence
    void HeavyAttackReleaseReadyAnimEvent()
    {
        CheckActions();

        if(isAttacking == true  && !isKnockdown)
        {
            releaseReady = true;
            sparkle.SetActive(true);
            GetComponent<SoundManager>().StopSound("Heavy Attack Charge");
            GetComponent<SoundManager>().PlaySound("Heavy Attack Ding");
            Debug.Log("ReadyToAttack");
        }
    }

    public void HeavyAtkRelease()
    {
        CheckActions();

        if (isAttacking == true && !isMoving)
        {
            if (releaseReady == true && !isKnockdown)
            {
                sparkle.SetActive(false);
                sword.GetComponent<WeaponDamage>().HeavyAttackDamage();
                anim.SetTrigger("HeavyAttackRelease");
                WeaponDamage.isAttacking = true;
                swordCollider.enabled = true;
                releaseReady = false;
                GetComponent<AttackAim>().Aim();
                GetComponent<SoundManager>().PlaySound("Sword Swing");
            }
            else
            {
                sparkle.SetActive(false);
                GetComponent<SoundManager>().StopSound("Heavy Attack Charge");
                releaseReady = false;
                anim.SetTrigger("HeavyAttackFail");
                isAttacking = false;
                sword.SetActive(false);
                sheathedSword.SetActive(true);
                PlayerController.isAttacking = false;
                
            }
        }
    }

    void HeavyAttackEndAnimEvent()
    {
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        swordCollider.enabled = false;
        WeaponDamage.isAttacking = false;
        PlayerController.isAttacking = false;
    }

    //stop charge attack bug when game paused
    public void CancelAttackOnPause()
    {
        if(releaseReady == true)
        {
            GetComponent<SoundManager>().StopSound("Heavy Attack Charge");
            releaseReady = false;
            anim.SetTrigger("HeavyAttackFail");
            sword.SetActive(false);
            sheathedSword.SetActive(true);
            PlayerController.isAttacking = false;
        }
        
    }

    void HeavyAttackSwordColliderOn()
    {
        swordCollider.enabled = true;
    }

    void HeavyAttackSwordColliderOff()
    {
        swordCollider.enabled = false;
    }

    void CheckActions()
    {
        isAttacking = PlayerController.isAttacking;
        isRolling = PlayerController.isRolling;
        isDizzy = PlayerController.isDizzy;
        isGrounded = PlayerController.isGrounded;
        isMoving = PlayerController.isMoving;
        isKnockdown = PlayerController.isKnockdown;
    }

}
