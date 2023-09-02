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

    //plays audio, good for looping
    public void PlaySound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    //Stops active audio
    public void StopSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    //play a one shot that does not need to be interrupted
    public void PlayOneShotSound(string name)
    { 
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        //use oneshot so the audio clips don't cancel each other out
        s.source.PlayOneShot(s.clip, s.volume);
    }

    //plays audio on object that called it so it can be messed with
    public void PlaySoundOnObject(string name, GameObject gameObj)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source = gameObj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.group;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.panStereo = s.pan;
        s.source.Play();
    }

    //stops audio on object
    public void StopSoundOnObject(string name, GameObject gameObj)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source = gameObj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.group;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.panStereo = s.pan;
        s.source.Stop();
    }
}
