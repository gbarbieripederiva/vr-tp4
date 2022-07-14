using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserBehaviour : MonoBehaviour
{
    public float speed = 2.5f;
    public float range = 2.5f;
    public float wateringCanRotationSpeed = 2.5f;
    
    private GameObject _wateringCan;
    private GameObject _grabbedCoin;
    private Rigidbody _rigidbody;
    private AudioSource _footstepAudioSource;
    private Collider _raycastCollider;
    private bool _grabbingWateringCan;
    private float _wateringCanYAngle;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _footstepAudioSource = GetComponent<AudioSource>();
        _wateringCan = GameObject.FindWithTag("WateringCan");
    }

    void Update()
    {
        Move();
        UpdateRaycast();
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

        var myPos = transform.position;
        if (x != 0 || z != 0)
        {
            Walk(myPos, x, z);
        }
        else if (_footstepAudioSource.isPlaying)
        {
            _footstepAudioSource.Pause();
        }

        if (_grabbingWateringCan)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                RotateWateringCan(-wateringCanRotationSpeed);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                RotateWateringCan(wateringCanRotationSpeed);
            }
            
            MoveWateringCan(myPos);
        }
    }

    void Walk(Vector3 myPos, float x, float z)
    {
        float facing = Camera.main.transform.eulerAngles.y;
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        Vector3 myInputs = new Vector3(horizontalMovement, 0, verticalMovement);
        Vector3 myTurnedInputs = Quaternion.Euler(0, facing, 0) * myInputs;
        var newPos = myPos + myTurnedInputs * speed * Time.deltaTime;
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

    void RotateWateringCan(float x)
    {
        if (_wateringCanYAngle + x < 45)
        {
            if (_wateringCanYAngle + x > -60)
            {
                _wateringCanYAngle += x;
            }
            else
            {
                _wateringCanYAngle = -60;
            }
        }
        else
        {
            _wateringCanYAngle = 45;
        }
    }

    void MoveWateringCan(Vector3 myPos)
    {
        _wateringCan.transform.position = myPos + new Vector3(0.7f, 0.2f, 2f);
        var myRot = transform.eulerAngles;
        _wateringCan.transform.localEulerAngles = new Vector3(0,  _wateringCanYAngle, myRot.z);
    }

    void Interact()
    {
        if (_raycastCollider == null && !_grabbingWateringCan && _grabbedCoin == null)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_grabbingWateringCan)
            {
                DropWateringCan();
                return;
            } 
            if (_grabbedCoin != null)
            {
                DropCoin();
                return;
            }

            if (_raycastCollider.CompareTag("WateringCan"))
            {
                GrabWateringCan();
            } else if (_raycastCollider.CompareTag("Door"))
            {
                KnockDoor();
            } else if (_raycastCollider.CompareTag("coin"))
            {
                _grabbedCoin = _raycastCollider.gameObject;
                GrabCoin();
            }
        }
    }

    void UpdateRaycast()
    {
        Vector3 direction = Vector3.forward;
        Transform transform1;
        Ray theRay = new Ray(transform.position  + new Vector3(0, 0.5f, 0), (transform1 = transform).TransformDirection(direction * range));
        Debug.DrawRay(transform1.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(direction * range));

        if (Physics.Raycast(theRay, out RaycastHit hit, range))
        {
            _raycastCollider = hit.collider;
        }
        else
        {
            _raycastCollider = null;
        }
    }

    void DropWateringCan()
    {
        _grabbingWateringCan = false;
        
        var wateringCanRigidBody = _wateringCan.GetComponent<Rigidbody>();
        wateringCanRigidBody.isKinematic = false;
        wateringCanRigidBody.useGravity = true;
    }

    void GrabWateringCan()
    {
        _grabbingWateringCan = true;
        
        var wateringCanRigidBody = _wateringCan.GetComponent<Rigidbody>();
        wateringCanRigidBody.isKinematic = true;
        wateringCanRigidBody.useGravity = false;
        _wateringCanYAngle = 0;
    }

    void GrabCoin()
    {
        var coinRigidBody = _grabbedCoin.GetComponent<Rigidbody>();
        coinRigidBody.isKinematic = true;
        coinRigidBody.useGravity = false;
    }

    void DropCoin()
    {
        var wateringCanRigidBody = _grabbedCoin.GetComponent<Rigidbody>();
        wateringCanRigidBody.isKinematic = false;
        wateringCanRigidBody.useGravity = true;
        _grabbedCoin = null;
    }

    void KnockDoor()
    {
        var door = _raycastCollider.gameObject.GetComponent<Door>();
        door.Knock();
    }
}
