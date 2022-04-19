using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeavyAttack : MonoBehaviour
{
    //Animation
    Animator anim;

    public PlayerInputActions playerInput;

    InputAction heavyAtkCharge;

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

    bool releaseReady = false;
    float chargeTimer;
    bool timerOn;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        playerInput = new PlayerInputActions();

        playerInput.Player.HeavyAtkCharge.performed += HeavyAtkCharge;
        playerInput.Player.HeavyAtkRelease.performed += HeavyAtkRelease;

        heavyAtkCharge = playerInput.Player.HeavyAtkCharge;

        swordCollider = sword.GetComponent<Collider>();
    }

    private void Update()
    {

        isAttacking = PlayerController.isAttacking;
        isRolling = PlayerController.isRolling;
        isDizzy = PlayerController.isDizzy;
        isGrounded = PlayerController.isGrounded;
        isMoving = PlayerController.isMoving;

        if (timerOn)
        {
            chargeTimer += Time.deltaTime;
        }
        else
        {

        }
        

    }

    void HeavyAtkCharge(InputAction.CallbackContext HeavyAtkCharge)
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

    void HeavyAtkRelease(InputAction.CallbackContext HeavyAtkRelease)
    {
        if (isAttacking == true && !isMoving)
        {
            if (releaseReady == true)
            {
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

    public void EnableHeavyAttackCharge()
    {
        heavyAtkCharge.Enable();
    }

    public void DisableHeavyAttackCharge()
    {
        heavyAtkCharge.Disable();
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
