using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodenBox : MonoBehaviour
{
    public GameObject coinPrefab;
    public Vector3 coinSpawnPoint =  new Vector3(394.8061f,1.1f,390.9682f);
    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("WateringCanFaucetCollider"))
        if(true)
        {
            _EliminateVegetableAndGenerateCoin(other.gameObject);
        }
    }

    private void _EliminateVegetableAndGenerateCoin(GameObject other)
    {
        Destroy(other);
        var coinObj = Instantiate(this.coinPrefab, this.coinSpawnPoint, Quaternion.identity);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
