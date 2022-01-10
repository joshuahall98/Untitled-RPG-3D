using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    //Call from Enemy/Player scripts to manage health
        public void StartHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthChange;
    }

    //Will trigger the health change only when taking Damage or healing
    private void HealthChange(object sender, System.EventArgs e)
    {
        transform.Find("ActualHealth").localScale = new Vector3(healthSystem.GetHealthPercentage(), 1);
    }

   
}
