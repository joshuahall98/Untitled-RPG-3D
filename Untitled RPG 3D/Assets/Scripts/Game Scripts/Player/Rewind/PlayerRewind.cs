using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static int rewindFadedHealth;

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;

    //action checkers
    public static int rewindsLeft = 4;
    public static int maxRewinds = 4;
    public static float rewindFillXPAmount;

    //UI
    public GameObject rewindUI;

    //action checkers
    public bool isRolling;
    public bool isAttacking;
    public bool isDead;

    //animation variables
    Animator anim;
    public GameObject hourGlass;

    private void Awake()
    {
        rewindUI = GameObject.Find("PlayerUI");

        anim = GetComponent<Animator>();

        hourGlass.SetActive(false);
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
            rewindUI.GetComponent<RewindUI>().FadedHealthBar();
           // rewindFadedHPBar.SetHealth(rewindFadedHealth);

        }

        //prevent you from exceeding max rewinds
        if (rewindsLeft > maxRewinds)
        {
            rewindsLeft = maxRewinds;
        }

        //make sure rewinds don't fall below 0
        if (rewindsLeft <= 0)
        {
            rewindsLeft = 0;
        }
   
        //stop xp increasing when you have max rewinds
        if(rewindsLeft == maxRewinds)
        {
            rewindFillXPAmount = 0;
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
        anim.SetBool("isRewinding", true);
        PlayerController.isAttacking = true;
        hourGlass.SetActive(true);


        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRewindStart"))
        {
            anim.SetBool("isRewinding", false);

            //call the UI update when you rewind
            rewindUI.GetComponent<RewindUI>().Rewind();

            //decreasing rewinds
            rewindsLeft -= 1;

            //takes point of time from array and sets player to position
            PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;

            //updating the players health that it has healed
            playerHealth = pointInTime.hp;
            PlayerHealth.currentHP = playerHealth;
            rewindUI.GetComponent<RewindUI>().HealthBar();

            //empty rewind array to start fresh
            pointsInTime.Clear();
            Rewinding = false;
            hourGlass.SetActive(false);
        }
  
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
                        if(rewindsLeft > 0)
                        {
                            Rewinding = true;
                        }
                        

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
    public void GetXPAmount()
    {
        float random = Random.Range(0.1f, 0.4f);
        rewindFillXPAmount = rewindFillXPAmount + random;

    }

    public void EndRewindAnimEvent()
    {
        PlayerController.isAttacking = false;
    }

}

