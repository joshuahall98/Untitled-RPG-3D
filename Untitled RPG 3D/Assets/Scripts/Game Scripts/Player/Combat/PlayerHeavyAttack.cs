using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : MonoBehaviour
{
    //Animation
    Animator anim;

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

    private void Update()
    {
        Knockeddown();
    }


    public void HeavyAtkCharge()
    {
        
        anim.SetTrigger("HeavyAttackHold");
        anim.ResetTrigger("HeavyAttackFail");
        sword.GetComponent<MeshRenderer>().enabled = true;
        sword.GetComponent<BoxCollider>().enabled = true;
        sheathedSword.SetActive(false);
        swordCollider.enabled = false;
        releaseReady = false;
        SoundManager.SoundManagerInstance.SelectAudioClass("Player");
        SoundManager.SoundManagerInstance.PlaySound("Heavy Attack Charge");

    }

    //this line of code checks to see if the animation has reached full charge before allowing the swing to commence
    void HeavyAttackReleaseReadyAnimEvent()
    {

        if (PlayerController.state == PlayerState.ATTACKING)
        {
            releaseReady = true;
            sparkle.SetActive(true);
            //audio
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.StopSound("Heavy Attack Charge");
            SoundManager.SoundManagerInstance.PlaySound("Heavy Attack Ding");

        }

    }

    //when you release the mouse button this code runs to check the stage of the charge up
    public void HeavyAtkRelease()
    {
        if (releaseReady == true && PlayerController.state != PlayerState.KNOCKEDDOWN)
        {
            sparkle.SetActive(false);
            sword.GetComponent<WeaponDamage>().HeavyAttackDamage();
            anim.SetTrigger("HeavyAttackRelease");
            WeaponDamage.isAttacking = true;
            swordCollider.enabled = true;
            releaseReady = false;
            GetComponent<AttackIndicator>().Aim();
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.PlaySound("Sword Swing");
        }
        else
        {
            sparkle.SetActive(false);
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.StopSound("Heavy Attack Charge");
            releaseReady = false;
            anim.SetTrigger("HeavyAttackFail");

            //If player were to be knockeddown during an attack the idle would trigger overidiing knockdown
            if (PlayerController.state != PlayerState.KNOCKEDDOWN)
            {
                PlayerController.state = PlayerState.IDLE;
            }

            sword.GetComponent<MeshRenderer>().enabled = false;
            sword.GetComponent<BoxCollider>().enabled = false;
            sheathedSword.SetActive(true);

        }
    }

    public void HeavyAttackEndAnim()
    {
        sword.GetComponent<MeshRenderer>().enabled = false;
        sword.GetComponent<BoxCollider>().enabled = false;
        sheathedSword.SetActive(true);
        swordCollider.enabled = false;
        WeaponDamage.isAttacking = false;

        //If player were to be knockeddown during an attack the idle would trigger overidiing knockdown
        if (PlayerController.state != PlayerState.KNOCKEDDOWN)
        {
            PlayerController.state = PlayerState.IDLE;
        }

        //to check player rotation for roll
        this.GetComponent<PlayerController>().RollDirection();
    }

    void Knockeddown()
    {
        if (PlayerController.state == PlayerState.KNOCKEDDOWN)
        {
            sparkle.SetActive(false);
            SoundManager.SoundManagerInstance.SelectAudioClass("Player");
            SoundManager.SoundManagerInstance.StopSound("Heavy Attack Charge");
            releaseReady = false;
            anim.SetTrigger("HeavyAttackFail");
            sword.GetComponent<MeshRenderer>().enabled = false;
            sword.GetComponent<BoxCollider>().enabled = false;
            sheathedSword.SetActive(true);
        }
    }

    //stop charge attack bug when game paused 
    //need to test, certain it would be better to just run HeavyAtkRelease()
    /*public void CancelAttackOnPause()
    {
        if(releaseReady == true)
        {
            GetComponent<SoundManager>().StopSound("Heavy Attack Charge");
            releaseReady = false;
            anim.SetTrigger("HeavyAttackFail");
            sword.SetActive(false);
            sheathedSword.SetActive(true);
            //PlayerController.isAttacking = false;

            PlayerController.state = PlayerState.IDLE;
        }
        
    }*/

    void HeavyAttackSwordColliderOn()
    {
        swordCollider.enabled = true;
    }

    void HeavyAttackSwordColliderOff()
    {
        swordCollider.enabled = false;
    }

}
