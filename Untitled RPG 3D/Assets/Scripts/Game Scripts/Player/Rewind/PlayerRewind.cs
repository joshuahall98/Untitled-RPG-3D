using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerRewind : MonoBehaviour, CooldownActive
{

    //Cooldown
    [SerializeField] private CooldownSystem cooldownSystem;
    private string Id = "Rewind";
    private float CooldownDuration = 5;
    public string id => Id;
    public float cooldownDuration => CooldownDuration;

    //Used to record the no. of seconds before rewind
    private int RecordRewindTime = 5;

    //variables for keeping track of player health
    public int playerHealth;
    public int rewindFadedHealth;

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;
    public int rewindsLeft = 4;
    public int maxRewinds = 4;

    //UI
    public Image[] rewindFill;
    public Image[] rewindContainer;
    public float rewindFillXPAmount;
    //these two ints are used to figure out which container you are currently trying to fill up
    int j = 0;
    int k = 0;
    float random;
    public Image currentContainer;
    public Image currentFill;
    public Image refillingContainer;
    public HealthBar healthBar;
    public HealthBar rewindFadedHPBar;

    //inputs
    public PlayerInputActions playerInput;
    InputAction rewind;

    //action checkers
    public bool isRolling;
    public bool isAttacking;
    public bool isDead;

    private void Awake()
    {
        playerInput = new PlayerInputActions();

        playerInput.Player.Rewind.performed += rewindPerformed => PlsRewind();

        rewind = playerInput.Player.Rewind;
    }

    void Start()
    {
        pointsInTime = new List<PointInTime>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //making sure action checkers correspond with player controller
        isRolling = PlayerController.isRolling;
        isAttacking = PlayerController.isAttacking;
        isDead = PlayerController.isDead;

        

        //Run rewind function if variables are met 
        if (Rewinding == true && rewindsLeft > 0)
        {
            Rewind();

        }
        else
        {
            Record();

        }

        void Record()
        {

            //Keep log of the last x seconds delete everything else
            if (pointsInTime.Count > Mathf.Round(RecordRewindTime / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            //making sure rewind system keeps track of player's HP
            playerHealth = PlayerHealth.currentHP;

            //storing all variables in the array
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, playerHealth));

            //show the player how much health they can heal
            PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
            rewindFadedHealth = pointInTime.hp;
            rewindFadedHPBar.SetHealth(rewindFadedHealth);

        }

        //Rewind UI hour glass fill
        if (rewindsLeft > maxRewinds)
        {
            rewindsLeft = maxRewinds;
        }

        //this fills up the rewind containers
        for (int i = 0; i < rewindFill.Length; i++)
        {
            if(i < rewindsLeft)
            {
                currentFill = rewindFill[i];
                currentFill.fillAmount = 1;
            }

        }

        // this makes sure the max rewind UI matches max rewinds
        for (int i = 0; i < rewindContainer.Length; i++)
        {
            if (i < maxRewinds)
            {
                currentContainer = rewindContainer[i];
                currentContainer.enabled = true;
            }
            else
            {
                currentContainer = rewindContainer[i];
                currentFill = rewindFill[i];
                currentFill.enabled = false;
                currentContainer.enabled = false;
                
            }

        }



        //make sure rewinds don't fall below 0
        if (rewindsLeft <= 0)
        {
            rewindsLeft = 0;
        }

        //filling up the hour glass
        if (rewindsLeft < maxRewinds)
        {
            refillingContainer = rewindFill[k];
            refillingContainer.fillAmount = rewindFillXPAmount;
        }

        //when you fill up a rewind container start the count from scratch
        if(rewindFillXPAmount >= 1)
        {
            rewindFillXPAmount = 0;
            rewindsLeft++;
            j = j - 1;
            k = rewindFill.Length - j;

        }

        //can't fill up if dead
        if (isDead)
        {
            rewindFillXPAmount = 0;
        }

    }

    //rewind player to point x seconds ago
    void Rewind()
    {
        //xp system
        j = j + 1;
        k = rewindsLeft - j;
        currentFill = rewindFill[k];
        currentFill.fillAmount = 0;
        if (rewindsLeft == maxRewinds)
        {
            rewindFillXPAmount = 0;
        }

        //decreasing rewinds
        rewindsLeft -= 1;

        
        

        //takes point of time from array and sets player to position
        PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
        transform.position = pointInTime.position;
        transform.rotation = pointInTime.rotation;

        //updating the players health that it has healed
        playerHealth = pointInTime.hp;
        PlayerHealth.currentHP = playerHealth;
        healthBar.SetHealth(playerHealth);

        //empty rewind array to start fresh
        pointsInTime.Clear();
        Rewinding = false;
       
    }

    //Rewind button function
    public void PlsRewind()
    {
        if (!isDead)
        {
            if (!isAttacking)
            {
                if (!isRolling)
                {
                    if (cooldownSystem.IsOnCooldown(id)) { return; }
                    {
                        Rewinding = true;

                        cooldownSystem.PutOnCooldown(this);
                    }
                }
            }
        }
        
    }

    //increase the amount of rewinds you have
    public void IncreaseRewinds()
    {
        rewindsLeft++;
    }

    //random xp amount
    public void XPAmount()
    {
        random = Random.Range(0.1f, 0.4f);
        rewindFillXPAmount = rewindFillXPAmount + random;

    }

    //disable rewind input
    public void DisableRewind()
    {
        rewind.Disable();
    }

    //enable rewind input
    public void EnableRewind()
    {
        rewind.Enable();
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

