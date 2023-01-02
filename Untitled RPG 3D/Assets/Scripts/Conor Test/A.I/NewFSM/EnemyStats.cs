using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }
}
