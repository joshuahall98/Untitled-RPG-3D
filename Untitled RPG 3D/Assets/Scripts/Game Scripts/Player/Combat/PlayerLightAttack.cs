using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
{
    //Animation
    Animator anim;

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

        if (atkNum == 0 || atkNum == 2)
        {
            sword.GetComponent<WeaponDamage>().LightAttackDamage();
            GetComponent<AttackAim>().Aim();
            WeaponDamage.isAttacking = true;
            sword.SetActive(true);
            sheathedSword.SetActive(false);
            atkNum++;
            swingCDTimer = 1;
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.PlaySound("Sword Swing");
            anim.SetTrigger("LightAttack1");

        }
        else
        {
            sword.GetComponent<WeaponDamage>().LightAttackDamage();
            GetComponent<AttackAim>().Aim();
            WeaponDamage.isAttacking = true;
            sword.SetActive(true);
            sheathedSword.SetActive(false);
            atkNum++;
            swingCDTimer = 1;
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.PlaySound("Sword Swing");
            anim.SetTrigger("LightAttack2");
        }
    }

    //ending the attack animation as an animation event
    IEnumerator LightAttackEndAnimEvent()
    {
        
        WeaponDamage.isAttacking = false;
        sword.SetActive(false);
        sheathedSword.SetActive(true);

        //If player were to be knockeddown during an attack the idle would trigger overidiing knockdown
        if(PlayerController.state != PlayerState.KNOCKEDDOWN)
        {
            PlayerController.state = PlayerState.IDLE;
        }

        //make sure player has position for roll
        this.GetComponent<PlayerController>().RollDirection();


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
