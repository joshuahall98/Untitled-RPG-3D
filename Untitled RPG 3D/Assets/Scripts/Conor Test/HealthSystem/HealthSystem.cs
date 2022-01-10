using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public float health;
    private int maxHealth;

    //Can scale in % rather than ints
    public float GetHealthPercentage()
    {
        return (float) health / maxHealth;
    }
    //Setting Max Health
    public HealthSystem(int maxHealth)
    {
        this.health = maxHealth;
        health = maxHealth;
    }

    //Take Damage -  catching when health = 0
    public void Damage(int dmg)
    {
        health -= dmg;
        if(health < 0)
        {
            health = 0;
        }
        //If this event is triggered
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    //Healing - catching when at max health
    public void Healing(int heal)
    {
        health += heal;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

}

   
