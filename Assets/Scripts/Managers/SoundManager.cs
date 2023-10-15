using System;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static public SoundManager Instance { get; private set; }

    private AudioSource audioSource;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip soundfile)
    {
        audioSource.PlayOneShot(soundfile);
    }

}
