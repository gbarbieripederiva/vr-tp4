using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyVegetable : MonoBehaviour
{
    public int coinsToBuy = 10;
    public GameObject boughtObject;
    private int coins = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("coin"))
        {
            _Buy(other.gameObject);
        }
    }

    void _Buy(GameObject coin){
        Destroy(coin);
        coins += 1;
        if(coins > coinsToBuy){
            Instantiate(boughtObject, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
            coins -= coinsToBuy;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
