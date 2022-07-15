using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodenBox : MonoBehaviour
{
    public GameObject coinPrefab;
    public Vector3 coinSpawnPoint =  new Vector3(394.8061f,1.3f,390.9682f);

    private Vector3 randomPosition =  new Vector3(1f,0.2f,1f);
    
    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("WateringCanFaucetCollider"))
        if(true)
        {
            _EliminateVegetableAndGenerateCoins(other.gameObject);
        }
    }

    private void _EliminateVegetableAndGenerateCoins(GameObject other)
    {
        int reward = other.GetComponent<Vegetable>().GetReward();
        Destroy(other);
        
        // TODO: Ver si hay que poner un delay, que onda las superposiciones de monedas, etc
        for (int i = 0; i < reward; i++) {
            Instantiate(coinPrefab, coinSpawnPoint + randomPosition * Random.Range(-1f, 1f), Quaternion.identity);
        }
    }
}
