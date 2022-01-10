using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSystem : MonoBehaviour
{
    private readonly List<CooldownData> cooldowns = new List<CooldownData>();

    private void Update()
    {
        ProcessCooldowns();
    }
    public void PutOnCooldown(CooldownActive cooldown)
    {
        cooldowns.Add(new CooldownData(cooldown));
    }
    public bool IsOnCooldown(string id)
    {
        foreach(CooldownData cooldown in cooldowns)
        {
            if(cooldown.id == id) { return true; }
        }

        return false;
    }

    public float GetRemaningDuration(string id)
    {
        foreach (CooldownData cooldown in cooldowns)
        {
            if (cooldown.id != id) { continue; }
            return cooldown.remainingTime;
        }

        return 0f;

    }

    private void ProcessCooldowns()
    {
        float deltaTime = Time.deltaTime;

        for (int i = cooldowns.Count - 1; i >= 0; i--)
        {
            if (cooldowns[i].DecrementCooldown(deltaTime))
            {
                cooldowns.RemoveAt(i);
            }
        }
    }
}
public class CooldownData
{
    public CooldownData(CooldownActive cooldown)
    {
        //GetCooldown
        id = cooldown.id;
        remainingTime = cooldown.cooldownDuration;
    }
    public string id { get; }

    public float remainingTime { get; private set; }

    //Method for DecrementCooldown check return value for True or False
    public bool DecrementCooldown(float deltaTime)
    {
        remainingTime = Mathf.Max(remainingTime - deltaTime, 0f);

        return remainingTime == 0f;
   }

}
