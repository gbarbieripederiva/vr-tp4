using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour
{
    public Animator handleAnimator;
    public float fillEverySeconds = 0.3f;
    public float fillAmount = 0.05f;
    
    private float _lastFilled;
    private bool _opening;
    private bool _closing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WateringCanFaucetCollider"))
        {
            _openFaucet(other.gameObject.GetComponentInParent<WaterCan>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("WateringCanFaucetCollider"))
        {
            _fillWateringCan(other.gameObject.GetComponentInParent<WaterCan>());
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WateringCanFaucetCollider"))
        {
            _closeFaucet();
        }
    }

    private void _openFaucet(WaterCan waterCan)
    {
        if (!waterCan.IsFull())
        {
            handleAnimator.SetBool("open", true);
        }
    }

    private void _fillWateringCan(WaterCan waterCan)
    {
        if (Time.time - _lastFilled > fillEverySeconds)
        {
            waterCan.Add(fillAmount);
            _lastFilled = Time.time;
        }
        
        if (waterCan.IsFull())
        {
            _closeFaucet();
        }
    }

    private void _closeFaucet()
    {
        handleAnimator.SetBool("open", false);
    }
}
