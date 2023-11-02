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

    //private AudioSource[] allAudio;

    public static event Action stopAllAudio;

    private void Awake()
    {
        SoundManagerInstance = this;   
    }

    private void Start()
    {
        /*allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];//store all audio sources in array

        foreach (AudioSource source in allAudio)
        {
            Debug.Log(source);
        }*/

        sounds = arrayStorage.ToArray();//store all the list values to an array

    }

    //The sound disks call this to add their sounds to the audio list
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
            s.source.spatialBlend = s.spatialBlend;
        }
    }


    //stops all audio when called
    public void StopAllAudio()
    {

        stopAllAudio();

        /*foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }*/
    }

    //plays single instance of Audio
    public void PlaySound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    //plays single instance of menu Audio
    public void PlaySoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
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

    //Stops single instance of menu Audio
    public void StopSoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
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

    //play a one shot that does not need to be interrupted in the menu, can have mutiple instances
    public void PlayOneShotSoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
        s.source.PlayOneShot(s.clip, s.volume);
    }

    //rework this to switch between the two
    public void PauseAllAudio()
    {
        AudioListener.pause = true;
    }

    /*  //plays audio on the object that it is called at, this allows us to play mutiple instances of the same sound
      public void PlaySoundOnObject(string name, GameObject gameObj)
      {
          Destroy(gameObj.GetComponent<AudioSource>());//delete if one exists
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

          Destroy(gameObj.GetComponent<AudioSource>());
      }*/


}
