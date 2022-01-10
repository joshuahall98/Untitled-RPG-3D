using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour, CooldownActive
{
    [SerializeField] private CooldownSystem CooldownSytem;
    private string Id = "Rewind";
    [SerializeField] private float CooldownDuration;

    public string id => Id;
    public float cooldownDuration => CooldownDuration;
    // Start is called before the first frame update

    public void BasicAttk()
    {

        if (CooldownSytem.IsOnCooldown(id)) { return; }
        {

            CooldownSytem.PutOnCooldown(this);
        }

    }
}
