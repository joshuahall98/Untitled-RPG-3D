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

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    Collider swordCollider;

    public bool releaseReady = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        swordCollider = sword.GetComponent<Collider>();
    }

    private void Update()
    {

        isAttacking = PlayerController.isAttacking;
        isRolling = PlayerController.isRolling;
        isDizzy = PlayerController.isDizzy;
        isGrounded = PlayerController.isGrounded;
        isMoving = PlayerController.isMoving;   

    }

    public void HeavyAtkCharge()
    {
        if (!isRolling)
        {
            if (!isDizzy)
            {
                if (isGrounded)
                {
                    if (!isAttacking)
                    {
                        anim.SetTrigger("HeavyAttackHold");
                        anim.ResetTrigger("HeavyAttackFail");
                        isAttacking = true;
                        PlayerController.isAttacking = true;
                        sword.SetActive(true);
                        sheathedSword.SetActive(false);
                        swordCollider.enabled = false;
                        releaseReady = false;

                    }
                }
            }
        }


    }

    //this line of code checks to see if the animation has reached full charge before allowing the swing to commence
    void HeavyAttackReleaseReadyAnimEvent()
    {
        releaseReady = true;
        Debug.Log("ReadyToAttack");
        if (releaseReady == false)
        {
            isAttacking = false;
            isMoving = false;
            isRolling = false;
        }

    }

    public void HeavyAtkRelease()
    {
        if (isAttacking == true && !isMoving)
        {
            if (releaseReady == true)
            {
                sword.GetComponent<WeaponDamage>().HeavyAttackDamage();
                anim.SetTrigger("HeavyAttackRelease");
                WeaponDamage.isAttacking = true;
                swordCollider.enabled = true;
                releaseReady = false;
                GetComponent<AttackAim>().Aim();
                GetComponent<AttackDash>().Dash();
            }
            else
            {
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
        isAttacking = false;
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        swordCollider.enabled = false;
        WeaponDamage.isAttacking = false;
        PlayerController.isAttacking = false;
    }

    void HeavyAttackSwordColliderOn()
    {
        swordCollider.enabled = true;
    }

    void HeavyAttackSwordColliderOff()
    {
        swordCollider.enabled = false;
    }

}
