using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundLooper : MonoBehaviour
{
    public float everySeconds = 80f;
    public float dispersionTimeRange = 20f;
    
    private AudioSource _audioSource;
    private bool _playing;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _playing = false;
    }

    void Update()
    {
        if (_playing || _audioSource.isPlaying)
        {
            return;
        }

        _playing = true;
        Invoke("PlaySound", Random.Range(everySeconds - dispersionTimeRange, everySeconds + dispersionTimeRange));
    }

    void PlaySound()
    {
        _audioSource.Play();
        _playing = false;
    }
}
