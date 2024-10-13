using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;

    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlaySound(string soundName)
    {
        Sound sfx = System.Array.Find(sounds, sound => sound.name == soundName);
        if (sfx != null && sfx.clip != null)
        {
            audioSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
        }
    }
}


