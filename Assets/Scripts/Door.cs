using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource _knockingAudioSource;

    void Start()
    {
        _knockingAudioSource = GetComponent<AudioSource>();
    }

    public void Knock()
    {
        if (!_knockingAudioSource.isPlaying)
        {
            _knockingAudioSource.Play();
        }
    }
}
