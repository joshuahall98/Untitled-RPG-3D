using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightAttack : MonoBehaviour
{
    //Animation
    Animator anim;

    public PlayerInputActions playerInput;

    InputAction lightAtk;

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

    public float swingCDTimer = 0;
    public int atkNum = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        playerInput.Player.Attack.performed += LightAtk;

        lightAtk = playerInput.Player.Attack;

        swordCollider = sword.GetComponent<Collider>();

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

    void LightAtk(InputAction.CallbackContext attk)
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
                            anim.SetTrigger("LightAttack1");
                            GetComponent<AttackAim>().Aim();
                            WeaponDamage.isAttacking = true;
                            PlayerController.isAttacking = true;
                            sword.SetActive(true);
                            sheathedSword.SetActive(false);
                            atkNum++;
                            swingCDTimer = 1;
                            GetComponent<AttackDash>().Dash();
                            
                        }
                        else
                        {
                            anim.SetTrigger("LightAttack2");
                            GetComponent<AttackAim>().Aim();
                            WeaponDamage.isAttacking = true;
                            PlayerController.isAttacking = true;
                            sword.SetActive(true);
                            sheathedSword.SetActive(false);
                            atkNum++;
                            swingCDTimer = 1;
                            GetComponent<AttackDash>().Dash();
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
            lightAtk.Disable();

            yield return new WaitForSeconds(1);

            lightAtk.Enable();
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

    public void EnableAttack()
    {
        lightAtk.Enable();
    }

    public void DisableAttack()
    {
        lightAtk.Disable();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

}
