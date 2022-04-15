using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightAttack : MonoBehaviour
{
    //Animation
    Animator anim;

    public PlayerInputActions playerInput;

    //weapons
    public GameObject sword;
    public GameObject sheathedSword;

    public static bool isAttacking;
    public static bool isRolling;

    public bool isAttackPub;
    public bool isRollingPub;

    public float atkCDTimer = 0;
    public int atkNum = 0;

    InputAction lightAtk;


    private void Awake()
    {
        playerInput = new PlayerInputActions();

        playerInput.Player.Attack.performed += LightAtk;

        lightAtk = playerInput.Player.Attack;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //weapons
        sword.SetActive(false);
        sheathedSword.SetActive(true);

    }

    private void Update()
    {
        //this prevents you from rolling too much
        if (atkCDTimer > 0)
        {
            atkCDTimer -= Time.deltaTime;
        }
        else if (atkCDTimer <= 0)
        {
            atkNum = 0;
            atkCDTimer = 0;
        }

        isAttackPub = isAttacking;
        isRollingPub = isRolling;
    }


    void LightAtk(InputAction.CallbackContext attk)
    {

        if (!isAttacking && !isRolling) {
            if (atkNum == 0)
            {
                anim.SetTrigger("LightAttack1");
                atkNum++;
                atkCDTimer = 3;
                lightAtk.Disable();
                isAttacking = true;
                PlayerController.isAttacking = true;
                PlayerHeavyAttack.isAttacking = true;
            }
            else
            {
                anim.SetTrigger("LightAttack2");
                atkNum = 0;
                lightAtk.Disable();
                isAttacking = true;
                PlayerController.isAttacking = true;
                PlayerHeavyAttack.isAttacking = true;
            }
        }
        
        
    }

    //starting the attack animation
    void LightAttackStartAnimEvent()
    {

        GetComponent<PlayerHeavyAttack>().DisableHeavyAttackCharge();
        GetComponent<AttackAim>().Aim();
        WeaponDamage.isAttacking = true;
        sheathedSword.SetActive(false);
        sword.SetActive(true);

    }

    //ending the attack animation
    IEnumerator LightAttackEndAnimEvent()
    {
        lightAtk.Enable();
        PlayerController.isAttacking = false;
        PlayerHeavyAttack.isAttacking = false;
        WeaponDamage.isAttacking = false;
        isAttacking = false;
        sheathedSword.SetActive(true);
        sword.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        GetComponent<PlayerHeavyAttack>().EnableHeavyAttackCharge();

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
