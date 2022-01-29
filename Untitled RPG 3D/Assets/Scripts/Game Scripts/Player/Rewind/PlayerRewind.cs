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
    [SerializeField] private float CooldownDuration = 5;
    public string id => Id;
    public float cooldownDuration => CooldownDuration;

    //Used to record the no. of seconds before rewind
    [SerializeField] private int RecordRewindTime = 5;

    //variables for keeping track of player health
    public int playerHealth;
    public int rewindFadedHealth;

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;
    public int rewindsLeft = 4;
    public int maxRewinds = 4;
    public bool isRecording = true;

    //UI
    public Image[] rewindFill;
    public Image currentContainer;
    public HealthBar healthBar;
    public HealthBar rewindFadedHPBar;

    //inputs
    public PlayerInputActions playerInput;
    InputAction rewind;

    //action checkers
    [SerializeField] bool isRolling;
    [SerializeField] bool isAttacking;

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

        //Rewind UI hour glass
        if (rewindsLeft > maxRewinds)
        {
            rewindsLeft = maxRewinds;
        }

        for (int i = 0; i < rewindFill.Length; i++)
        {
            if(i < rewindsLeft)
            {
                //rewindsContainer[i].gameObject.SetActive(true);
                currentContainer = rewindFill[i];
                currentContainer.fillAmount = 1;
            }
            else
            {
                //rewindsContainer[i].gameObject.SetActive(false);
                currentContainer = rewindFill[i];
                currentContainer.fillAmount = 0;
            }
        }

        //make sure rewinds don't fall below 0
        if (rewindsLeft <= 0)
        {
            rewindsLeft = 0;
        }

    }

    //rewind player to point x seconds ago
    void Rewind()
    {
      
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
        if (!isAttacking)
        {
            if (!isRolling)
            {
                if (cooldownSystem.IsOnCooldown(id)) { return; }
                {

                    Rewinding = true;
                    rewindsLeft -= 1;

                    cooldownSystem.PutOnCooldown(this);
                }
            }
        }
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

