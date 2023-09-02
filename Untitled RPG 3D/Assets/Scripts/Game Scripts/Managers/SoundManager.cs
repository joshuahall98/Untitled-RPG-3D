using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Web.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

//Joshua

public class SoundManager : MonoBehaviour
{

    public static SoundManager SoundManagerInstance;

    //for storing currently used sounds
    private AudioScriptableObject[] sounds;

    private List<AudioScriptableObject> arrayStorage = new List<AudioScriptableObject>();

    private AudioSource[] allAudio;

    private void Awake()
    {
        SoundManagerInstance = this;
    }

    private void Start()
    {
        allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];//store all audio sources in array

        sounds = arrayStorage.ToArray();//store all the list values to an array
    }

    public void GenerateAudioComponentList(AudioScriptableObject[] audioList)
    {
        arrayStorage.AddRange(audioList);//add all the array values to a list
        //create all the audio sources
        foreach (AudioScriptableObject s in audioList)
        {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.group;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
        }
    }

    //stops all audio when called
    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }
    }

    public void PlaySound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void PlayOneShotSound(string name)
    { 
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        //use oneshot so the audio clips don't cancel each other out
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void StopSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    /*public static SoundManager SoundManagerInstance;

    [Header("PLAYER")]
    [SerializeField] AudioScriptableObject[] player;

    
    [Space]
    [Header("AI")]
    [SerializeField] AudioScriptableObject[] wurgle;

    //for storing currently used sounds
    AudioScriptableObject[] sounds;


    List<AudioScriptableObject[]> arrayStorage = new List<AudioScriptableObject[]>();

    private AudioSource[] allAudio;


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
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        //use oneshot so the audio clips don't cancel each other out
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void StopSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    //this creates all the audio components on the game manager
    void TheAudioComponentList()
    {

        for (int i = 0; i < arrayStorage.Count; i++)
        {
            foreach (AudioScriptableObject s in arrayStorage[i])
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

    }*/


}
