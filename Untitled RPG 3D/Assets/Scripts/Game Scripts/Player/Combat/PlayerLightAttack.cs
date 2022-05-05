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

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    Collider swordCollider;
    AudioSource audio;

    public float swingCDTimer = 0;
    public int atkNum = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        swordCollider = sword.GetComponent<Collider>();

        audio = sword.GetComponent<AudioSource>();

    }

    private void Update()
    {
        isAttacking = PlayerController.isAttacking;
        isRolling = PlayerController.isRolling;
        isDizzy = PlayerController.isDizzy;
        isGrounded = PlayerController.isGrounded;

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
                        if (atkNum == 0 || atkNum == 2)
                        {
                            sword.GetComponent<WeaponDamage>().LightAttackDamage();
                            anim.SetTrigger("LightAttack1");
                            GetComponent<AttackAim>().Aim();
                            WeaponDamage.isAttacking = true;
                            PlayerController.isAttacking = true;
                            sword.SetActive(true);
                            sheathedSword.SetActive(false);
                            atkNum++;
                            swingCDTimer = 1;
                            FindObjectOfType<SoundManager>().PlaySound("Sword Swing");
                            
                        }
                        else
                        {
                            sword.GetComponent<WeaponDamage>().LightAttackDamage();
                            anim.SetTrigger("LightAttack2");
                            GetComponent<AttackAim>().Aim();
                            WeaponDamage.isAttacking = true;
                            PlayerController.isAttacking = true;
                            sword.SetActive(true);
                            sheathedSword.SetActive(false);
                            atkNum++;
                            swingCDTimer = 1;
                            FindObjectOfType<SoundManager>().PlaySound("Sword Swing");
                        }
                    }
                }
            }
        }
    }

    //ending the attack animation
    IEnumerator LightAttackEndAnimEvent()
    {
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
