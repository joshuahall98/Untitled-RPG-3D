using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
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
    Collider swordCollider;

    public float swingCDTimer = 0;
    public int atkNum = 0;

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
        isKnockdown = PlayerController.isKnockdown;

        //cd for combo between swings
        if (swingCDTimer > 0)
        {
            swingCDTimer -= Time.deltaTime;
        }
        else if (swingCDTimer <= 0)
        {
            if(atkNum < 3)
            {
                atkNum = 0;
                swingCDTimer = 0;
            } 
        }

        //an attempt to refine combat
        /*if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1"))
        {
            StartCoroutine(LightAttackEndAnimEvent());
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack2"))
        {
            StartCoroutine(LightAttackEndAnimEvent());
        }*/
    }

    public void LightAtk()
    {
        if (!isRolling)
        {
            if (!isDizzy)
            {
                if (isGrounded)
                {
                    if (!isAttacking)
                    {
                        if (!isKnockdown)
                        {
                            if (atkNum == 0 || atkNum == 2)
                            {
                                //GetComponent<PlayerController>().DisableLightAttack();
                                sword.GetComponent<WeaponDamage>().LightAttackDamage();
                                GetComponent<AttackAim>().Aim();
                                WeaponDamage.isAttacking = true;
                                PlayerController.isAttacking = true;
                                sword.SetActive(true);
                                sheathedSword.SetActive(false);
                                atkNum++;
                                swingCDTimer = 1;
                                FindObjectOfType<SoundManager>().PlaySound("Sword Swing");
                                anim.SetTrigger("LightAttack1");

                            }
                            else
                            {
                                //GetComponent<PlayerController>().DisableLightAttack();
                                sword.GetComponent<WeaponDamage>().LightAttackDamage();
                                GetComponent<AttackAim>().Aim();
                                WeaponDamage.isAttacking = true;
                                PlayerController.isAttacking = true;
                                sword.SetActive(true);
                                sheathedSword.SetActive(false);
                                atkNum++;
                                swingCDTimer = 1;
                                FindObjectOfType<SoundManager>().PlaySound("Sword Swing");
                                anim.SetTrigger("LightAttack2");
                            }
                        }
                        
                    }
                }
            }
        }
    }

    //ending the attack animation as an animation event
    IEnumerator LightAttackEndAnimEvent()
    {
        //GetComponent<PlayerController>().EnableLightAttack();
        WeaponDamage.isAttacking = false;
        PlayerController.isAttacking = false;
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        //3 hit combo cd
        if (atkNum >= 3)
        {
            GetComponent<PlayerController>().DisableLightAttack();

            yield return new WaitForSeconds(1);

            GetComponent<PlayerController>().EnableLightAttack();
            atkNum = 0;
        }
    }

    void LightAttackSwordColliderOn()
    {
        swordCollider.enabled = true;
    }

    void LightAttackSwordCollideroff()
    {
        swordCollider.enabled = false;
    }


}
