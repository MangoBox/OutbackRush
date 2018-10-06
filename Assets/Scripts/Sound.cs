using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [Range(0.1f, 3f)]
    public float pitch;
    public float pitchVariation;
    [Range(0, 1)]
    public float volume;
    public bool loop;
    public bool playOnAwake;
    

    public string name;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource audioSource;
}