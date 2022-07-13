using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserBehaviour : MonoBehaviour
{
    public float speed = 2.5f;
    public float range = 2.5f;
    
    private GameObject _wateringCan;
    private Rigidbody _rigidbody;
    private AudioSource _footstepAudioSource;
    private Collider _raycastCollider;
    private bool _grabbingWateringCan;

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
        else
        {
            if (_footstepAudioSource.isPlaying)
            {
                _footstepAudioSource.Pause();
            }
        }

        if (_grabbingWateringCan)
        {
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

    void MoveWateringCan(Vector3 myPos)
    {
        _wateringCan.transform.position = myPos + new Vector3(0.7f, 0.4f, 1f);
        var myRot = transform.eulerAngles;
        _wateringCan.transform.eulerAngles = new Vector3(-90, 0, myRot.z + 90);
    }

    void Interact()
    {
        if (_raycastCollider == null && !_grabbingWateringCan)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (_grabbingWateringCan)
            {
                DropWateringCan();
                return;
            }

            if (_raycastCollider.CompareTag("WateringCan"))
            {
                GrabWateringCan();
            } else if (_raycastCollider.CompareTag("Door"))
            {
                KnockDoor();
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
    }

    void KnockDoor()
    {
        var door = _raycastCollider.gameObject.GetComponent<Door>();
        door.Knock();
    }
}
