using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] GameObject audioSources;

    [SerializeField] AudioScriptableObject[] sounds;

    private AudioSource[] allAudio;

    private void Awake()
    {
        GenerateAudioComponentList();

        SoundManager.stopAllAudio += StopAudio;

        allAudio = GetComponentsInChildren<AudioSource>();
    }

    void GenerateAudioComponentList()
    {
        //create all the audio sources
        foreach (AudioScriptableObject s in sounds)
        {
            s.source = audioSources.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.group;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
        }
    }

    //stop all audio event
    public void StopAudio()
    {
        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }
    }

    //https://forum.unity.com/threads/solved-c-when-does-an-array-become-too-big.606955/

    //plays Audio
    public void PlaySound(int index)
    {
        if (allAudio[index] == null)
            return;
        allAudio[index].Play();  
    }

    //Stops  Audio
    public void StopSound(int index)
    {
        if (allAudio[index] == null)
            return;
        allAudio[index].Stop();
    }

    //play a one shot that does not need to be interrupted, can have mutiple instances
    public void PlayOneShotSound(int index)
    {
        if (allAudio[index] == null)
            return;
        allAudio[index].PlayOneShot(allAudio[index].clip, allAudio[index].volume);
    }
}
