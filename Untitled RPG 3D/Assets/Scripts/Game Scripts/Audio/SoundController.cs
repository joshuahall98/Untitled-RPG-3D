using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    [SerializeField] AudioScriptableObject[] sounds;

    private AudioSource[] allAudio;

    private void Awake()
    {
        GenerateAudioComponentList();

        SoundManager.stopAllAudio += StopAudio;

        allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];//store all audio sources in array
    }

    void GenerateAudioComponentList()
    {
        //create all the audio sources
        foreach (AudioScriptableObject s in sounds)
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

    //stop all audio event
    private void StopAudio()
    {
        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }
    }

    //plays single instance of Audio
    public void PlaySound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    //Stops single instance of Audio
    public void StopSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    //play a one shot that does not need to be interrupted, can have mutiple instances
    public void PlayOneShotSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.PlayOneShot(s.clip, s.volume);
    }
}
