using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRewind : MonoBehaviour, CooldownActive
{

    //Cooldown
    [SerializeField] private CooldownSystem CooldownSytem;

    private string Id = "Rewind";
    [SerializeField] private float CooldownDuration;

    //Used to record the no. of seconds before rewind
    [SerializeField] private int RecordRewindTime;
    //Lambda declaration
    public string id => Id;
    public float cooldownDuration => CooldownDuration;

    public float health;

    //Rewind
    bool Rewinding = false;
    List<PointInTime> pointsInTime;
    public int rewindsLeft = 4;
    public int maxRewinds = 4;

    //UI
    public Slider[] rewindsContainer;

    void Start()
    {
        
        pointsInTime = new List<PointInTime>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Run rewind function if variables are met 
        if (Rewinding == true && rewindsLeft >= 0)
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

            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, health));

        }

        //Rewind UI sliders
        if(rewindsLeft > maxRewinds)
        {
            rewindsLeft = maxRewinds;
        }

        for (int i = 0; i < rewindsContainer.Length; i++)
        {
            if(i < rewindsLeft)
            {
                //rewindsContainer[i].gameObject.SetActive(true);
                rewindsContainer[i].value = 1;
            }
            else
            {
                //rewindsContainer[i].gameObject.SetActive(false);
                rewindsContainer[i].value = 0;
            }
        }

    }
    //rewind player to point x seconds ago
    void Rewind()
    {
      

        PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
        transform.position = pointInTime.position;
        transform.rotation = pointInTime.rotation;
        health = pointInTime.hp;
        pointsInTime.RemoveAt(pointsInTime.Count - 1);
        Rewinding = false;
       
    }

    //Rewind button function
    public void PlsRewind()
    {

        if (CooldownSytem.IsOnCooldown(id)) { return; }
        {
           
            Rewinding = true;
            rewindsLeft -= 1;
;
            CooldownSytem.PutOnCooldown(this);
        }

    }

}

