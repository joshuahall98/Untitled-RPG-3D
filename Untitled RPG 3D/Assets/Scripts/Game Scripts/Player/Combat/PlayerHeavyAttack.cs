using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeavyAttack : MonoBehaviour
{
    //Animation
    Animator anim;

    public PlayerInputActions playerInput;

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;
    Collider swordCollider;

    public static bool isAttacking;
    public static bool isRolling;

    public bool isAttackPub;
    public bool isRollingPub;

    InputAction heavyAtkCharge;

    bool mouseAim;
    bool releaseReady = false;

    private void Awake()
    {
        playerInput = new PlayerInputActions();

        playerInput.Player.HeavyAtkCharge.performed += HeavyAtkCharge;
        playerInput.Player.HeavyAtkRelease.performed += HeavyAtkRelease;

        heavyAtkCharge = playerInput.Player.HeavyAtkCharge;
    }

    private void Update()
    {
        if(mouseAim == true)
        {
            GetComponent<AttackAim>().Aim();
        }

        isAttackPub = isAttacking;
        isRollingPub = isRolling;

    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //weapons
        sword.SetActive(false);
        sheathedSword.SetActive(true);
        swordCollider = sword.GetComponent<Collider>();

    }

    void HeavyAtkCharge(InputAction.CallbackContext HeavyAtkCharge)
    {
        if (!isRolling && !isAttacking)
        {
            GetComponent<PlayerLightAttack>().DisableAttack();
            GetComponent<PlayerController>().DisableRoll();
            swordCollider.enabled = false;
            anim.SetTrigger("HeavyAttackHold");
            anim.ResetTrigger("HeavyAttackFail");
            mouseAim = true;
            PlayerController.isAttacking = true;
            PlayerLightAttack.isAttacking = true;
            WeaponDamage.isAttacking = true;
            sheathedSword.SetActive(false);
            sword.SetActive(true);
            releaseReady = false;
        }
        
        

    }

    //this line of code checks to see if the animation has reached full charge before allowing the swing to commence
    void HeavyAttackReleaseReadyAnimEvent()
    {
        releaseReady = true;
        Debug.Log("ReadyToAttack");

    }

    void HeavyAtkRelease(InputAction.CallbackContext HeavyAtkRelease)
    { 

        if (releaseReady == true)
        {
            swordCollider.enabled = true;
            anim.SetTrigger("HeavyAttackRelease");
            mouseAim = false;
            PlayerController.isAttacking = true;
            PlayerLightAttack.isAttacking = true;
            WeaponDamage.isAttacking = true;
            heavyAtkCharge.Disable();

        }
        else
        {
            mouseAim = false;
            anim.SetTrigger("HeavyAttackFail");
            PlayerController.isAttacking = false;
            PlayerLightAttack.isAttacking = false;
            WeaponDamage.isAttacking = false;
            sheathedSword.SetActive(true);
            sword.SetActive(false);
            releaseReady = false;
            GetComponent<PlayerLightAttack>().EnableAttack();
            GetComponent<PlayerController>().EnableRoll();
            swordCollider.enabled = true;
        }
    }


    IEnumerator HeavyAttackEndAnimEvent()
    {
        PlayerController.isAttacking = false;
        PlayerLightAttack.isAttacking = false;
        WeaponDamage.isAttacking = false;
        sheathedSword.SetActive(true);
        sword.SetActive(false);
        releaseReady = false;
        GetComponent<PlayerLightAttack>().EnableAttack();
        GetComponent<PlayerController>().EnableRoll();

        yield return new WaitForSeconds(0.5f);

        heavyAtkCharge.Enable();
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
