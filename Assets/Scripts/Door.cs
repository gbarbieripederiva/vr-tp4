using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource _knockingAudioSource;
    private Animator _animator;

    void Start()
    {
        _knockingAudioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_knockingAudioSource.isPlaying && _animator.GetBool("addScale"))
        {
            _animator.SetBool("addScale", false);
        }
    }

    public void Knock()
    {
        if (!_knockingAudioSource.isPlaying)
        {
            _knockingAudioSource.Play();
            Invoke(nameof(StartAnimation), 0.3f);
        }
    }

    public void StartAnimation()
    {
        _animator.SetBool("addScale", true);
    }

    public void StopAnimation()
    {
        _animator.SetBool("addScale", false);
    }
}
