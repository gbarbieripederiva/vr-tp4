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

    private List<GameObject> _currentPlantables = new List<GameObject>();
    private GameObject _wateringCan;
    private GameObject _grabbedCoin;
    private GameObject _grabbedVegetable;
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

        if (_grabbedCoin != null)
        {
            MoveCoin(myPos);
        }

        if (_grabbedVegetable != null)
        {
            MoveVegetable(myPos);
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

    void MoveCoin(Vector3 myPos)
    {
        _grabbedCoin.transform.position = myPos + new Vector3(0.7f, 0.2f, 2f);
        var myRot = transform.eulerAngles;
        _grabbedCoin.transform.localEulerAngles = new Vector3(0,  0, myRot.z);
    }

    void MoveVegetable(Vector3 myPos)
    {
        _grabbedVegetable.transform.position = myPos + new Vector3(0.7f, 0.2f, 2f);
        var myRot = transform.eulerAngles;
        _grabbedVegetable.transform.localEulerAngles = new Vector3(0,  0, myRot.z);
    }

    void Interact()
    {
        if (_raycastCollider == null && !_grabbingWateringCan && _grabbedCoin == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_grabbedVegetable != null && !_grabbedVegetable.GetComponent<Vegetable>().HasBeenPlanted() && _currentPlantables.Count > 0)
            {
                var plantable = _currentPlantables.First(o =>
                {
                    var plantable = o.GetComponent<Plantable>();
                    return !plantable.IsOccupied();
                });
                if (plantable != null)
                {
                    PlantVegetable(plantable);
                }
                return;
            }
            
            if (_grabbedVegetable == null && _currentPlantables.Count > 0)
            {
                var plantable = _currentPlantables.First(o =>
                {
                    var plantable = o.GetComponent<Plantable>();
                    return plantable.IsOccupied();
                });
                if (plantable != null)
                {
                    GrabPlantedVegetable(plantable);
                }
                return;
            }
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
            if (_grabbedVegetable != null)
            {
                DropVegetable();
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
            } else if (_raycastCollider.tag.StartsWith("Vegetable"))
            {
                _grabbedVegetable = _raycastCollider.gameObject;
                var veg = _grabbedVegetable.GetComponent<Vegetable>();
                if (!veg.IsPlanted())
                {
                    GrabTableVegetable();
                }
            }
        }
    }

    void UpdateRaycast()
    {
        Vector3 direction = Camera.main.transform.forward;
        Transform transform1;
        // Ray theRay = new Ray(Camera.main.transform.position + new Vector3(0f,0.5f,0f), (transform1 = Camera.main.transform).TransformDirection(direction * range));
        // Debug.DrawRay(transform1.position, Camera.main.transform.TransformDirection(direction * range),Color.white,10);
        Ray theRay =  Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

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

    void GrabTableVegetable()
    {
        var vegetable = _grabbedVegetable.GetComponent<Rigidbody>();
        vegetable.isKinematic = true;
        vegetable.useGravity = false;
    }

    void PlantVegetable(GameObject plantableObject)
    {
        var vegetable = _grabbedVegetable.GetComponent<Rigidbody>();
        vegetable.isKinematic = true;
        vegetable.useGravity = false;

        var plantable = plantableObject.GetComponent<Plantable>();
        _grabbedVegetable.GetComponent<Vegetable>().Plant(plantable);
        _grabbedVegetable = null;
    }

    void GrabPlantedVegetable(GameObject plantable)
    {
        plantable.GetComponent<Plantable>().RemovePlant();

        var rb = plantable.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        _grabbedVegetable = plantable;
    }

    void DropVegetable()
    {
        var vegetable = _grabbedVegetable.GetComponent<Rigidbody>();
        vegetable.isKinematic = false;
        vegetable.useGravity = true;
        _grabbedVegetable = null;
    }

    void KnockDoor()
    {
        var door = _raycastCollider.gameObject.GetComponent<Door>();
        door.Knock();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plantable"))
        {
            _currentPlantables.Append(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Plantable"))
        {
            _currentPlantables.Remove(other.gameObject);
        }
    }
}
