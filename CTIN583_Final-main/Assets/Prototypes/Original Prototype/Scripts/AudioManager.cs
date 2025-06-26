using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Audio1;
    [SerializeField] private AudioClip startAudio1;
    public AudioSource Audio2;
    [SerializeField] private AudioClip startAudio2;
    [Range(0f, 1f)] [SerializeField] private float volume; 

    // Singleton instance
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }

    private void Start()
    {
        Audio1.volume = volume;
        Audio2.volume = 0;

        Audio1.PlayOneShot(startAudio1);
        Audio2.PlayOneShot(startAudio2);

	    Audio1.PlayScheduled(AudioSettings.dspTime + startAudio1.length);
        Audio2.PlayScheduled(AudioSettings.dspTime + startAudio2.length);
    }
}
