using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyVegetable : MonoBehaviour
{
    public int coinsToBuy = 10;
    public GameObject boughtObject;
    public TextMeshPro coinsToBuyText;
    public TextMeshPro remainingCoinsText;
    private int _coins ;
    private float lastBought;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            _buy(other.gameObject);
        }
    }

    void _buy(GameObject coin){
        if (Time.time - lastBought <= 0.2f)
        {
            return;
        }

        lastBought = Time.time;
        Destroy(coin);
        _coins += 1;

        if (_coins >= coinsToBuy){
            _coins -= coinsToBuy;
            var newGameObject = Instantiate(boughtObject, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
            var rigidBody = newGameObject.GetComponentInChildren<Rigidbody>();
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
        _updateText();
    }

    void _updateText()
    {
        var count = (coinsToBuy - _coins);
        remainingCoinsText.text = count + " Coin" + (count == 1 ? "" : "s");
    }

    void Start()
    {
        _updateText();
        coinsToBuyText.text = "Costs: " + coinsToBuy + " Coin" + (coinsToBuy == 1 ? "" : "s");
    }
}
