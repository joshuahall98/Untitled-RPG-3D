using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua 2023/11/02

namespace AudioSystem
{
    public class SoundDisk : MonoBehaviour
    {

        [SerializeField] AudioScriptableObject[] sounds;

        private void Awake()
        {
            this.GetComponentInParent<AudioManager>().GenerateAudioComponentList(sounds);
        }

    }
}

