using UnityEngine.Audio;
using UnityEngine;

//Joshua

//Old sound script, now using scriptable objects
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    [Range(-1f, 1f)]
    public float pan;

    public bool loop;

    [HideInInspector]
    public AudioSource source;




}
