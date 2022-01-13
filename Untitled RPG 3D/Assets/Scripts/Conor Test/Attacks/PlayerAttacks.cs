using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour, CooldownActive
{
    [SerializeField] private CooldownSystem CooldownSytem;
    private string Id = "LightAttack";
    [SerializeField] private float CooldownDuration;

    //Attack Area   
    public Transform attkPos;
    public LayerMask Enemy;
    public float attkRange;
    public int damage;
    public string id => Id;
    public float cooldownDuration => CooldownDuration;
    // Start is called before the first frame update

    public void BasicAttk()
    {

        if (CooldownSytem.IsOnCooldown(id)) { return; }
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attkPos.position, attkRange, Enemy);
            for(int i=0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<HealthSystem>().Damage(damage);
            }
            CooldownSytem.PutOnCooldown(this);
        }

    }
     //Visuals for testing range
 private void OnDrawGizmosSelected()
 {   
     Gizmos.color = Color.magenta;
     Gizmos.DrawWireSphere(attkPos.position, attkRange);


 }

}
