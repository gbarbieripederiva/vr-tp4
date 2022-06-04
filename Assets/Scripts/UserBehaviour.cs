using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserBehaviour : MonoBehaviour
{
    public float speed = 2.5f;
    private Rigidbody _rigidbody;
    private AudioSource _footstepAudioSource;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _footstepAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
        Interact();
    }
    
    void Move()
    {
        float x, z;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            z = 1;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            z = -1;
        }
        else
        {
            z = 0;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            x = 1;
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            x = -1;
        }
        else
        {
            x = 0;
        }

        if (x != 0 || z != 0)
        {
            var pos = transform.position;
            var diff = new Vector3(x, 0, z).normalized * speed * Time.deltaTime;
            var newPos = pos + diff;
            if (newPos.y < -0.3f)
            {
                newPos.y = -0.3f;
            }
            if (newPos.y > 3)
            {
                newPos.y = 3;
            }
            
            _rigidbody.MovePosition(newPos);
            if (!_footstepAudioSource.isPlaying)
            {
                _footstepAudioSource.Play();
            }
        }
        else
        {
            if (_footstepAudioSource.isPlaying)
            {
                _footstepAudioSource.Pause();
            }
        }
    }

    void Interact()
    {
        
    }
}
