using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Web.UI;
using System.Collections.Generic;

//Joshua

public class SoundManager : MonoBehaviour
{

    public static SoundManager SoundManagerInstance;

    [Header("PLAYER")]
    [SerializeField] Sound[] player;

    
    [Space]
    [Header("AI")]
    [SerializeField] Sound[] wurgle;

    //for storing currently used sounds
    [Space]
    [Header("EMPTY FIELD")]
    [SerializeField]Sound[] sounds;

    List<Sound[]> arrayStorage = new List<Sound[]>();

    private AudioSource[] allAudio;


    //decide on scriptable, will look at later
    //[SerializeField]List<AudioScriptableObject> scriptObj = new List<AudioScriptableObject>();

    void Awake()
    {
        SoundManagerInstance = this;

        AddArrayToList();
        TheAudioComponentList();   
        
    }

    //stops all audio when called
    public void StopAllAudio()
    {
        //Debug.Log("stop please");
        allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }

    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        //use oneshot so the audio clips don't cancel each other out
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    //this creates all the audio components on the game manager
    void TheAudioComponentList()
    {
        for (int i = 0; i < arrayStorage.Count; i++)
        {
            foreach (Sound s in arrayStorage[i])
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.outputAudioMixerGroup = s.group;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.panStereo = s.pan;
            }
        }

    }

    //Call this function to select which class
    public void SelectAudioClass(string name)
    {
        if(name == "Player")
        {
            sounds = player;
        }
        else if(name == "Wurgle")
        {
            sounds = wurgle;
        }
        
    }

    //This stores all the audio arrays in a list so that the components can be added
    void AddArrayToList()
    {
        //Player
        arrayStorage.Add(player);

        //AI
        arrayStorage.Add(wurgle);
    }

    
}
