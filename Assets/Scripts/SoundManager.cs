using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public Toggle audioToggle;
    static bool audioAllowed = true;

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;

            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;// + UnityEngine.Random.Range(-s.pitchVariation, s.pitchVariation);
            s.audioSource.loop = s.loop;
            if (s.playOnAwake && audioAllowed)
            {
                s.audioSource.Play();
            }
        }
    }

    public void Play(string name)
    {
        if (!audioAllowed) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.audioSource.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.audioSource.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name);
        }
    }

    public void UpdateAudioCheckbox()
    {
        if (audioToggle.isOn)
        {
            audioAllowed = true;
            Play("SoundtrackAmbient");
        }
        else
        {
            audioAllowed = false;
            StopAll();
        }
    }
}
