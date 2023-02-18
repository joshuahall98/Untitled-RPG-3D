using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Web.UI;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [Header("PLAYER")]
    [SerializeField] Sound[] player;

    
    [Space]
    [Header("AI")]
    [SerializeField] Sound[] wurgle;
    [SerializeField] Sound[] wurl;

    
    [Space]
    [Header("EMPTY FIELD")]
    [SerializeField] Sound[] sounds;


    //decide on scriptable
    [SerializeField]List<AudioScriptableObject> scriptObj = new List<AudioScriptableObject>();

    //store all the arrays in an array at start, traverse array and run the for each with each list.

    // Start is called before the first frame update
    void Awake()
    {
        TheAudioList();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        //use oneshot so the audio clips don;t cancel each other out
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void SelectAudio(string name)
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

    void TheAudioList()
    {
        foreach (Sound s in wurgle)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.group;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
        }

        foreach (Sound s in player)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.group;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
        }

        foreach (Sound s in sounds)
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
