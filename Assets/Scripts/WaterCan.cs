using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterCan : MonoBehaviour
{
    public float dropEverySeconds = 0.3f;
    public float dropAmount = 0.05f;
    
    private float _level;
    private float _lastDropped;
    
    public void Update()
    {
        if (Time.time - _lastDropped < dropEverySeconds)
        {
            return;
        }
        
        var y = transform.localEulerAngles.y;
        if (y > 300)
        {
            // A number between 0 - 45
            var normalizedY = 345 - y;
            var minWater = normalizedY / 45f;
            if (minWater < _level)
            {
                Drop(dropAmount);
                _lastDropped = Time.time;
            }
        } else if (y > 35)
        {
            // A number between 0 - 15
            var normalizedY = y - 35;
            
            // We don't want to remove all water
            var maxWater = normalizedY / 45f;
            if (maxWater < _level)
            {
                Drop(dropAmount);
                _lastDropped = Time.time;
            }
        }
    }

    public bool IsFull()
    {
        return _level >= 1;
    }

    public bool IsEmpty()
    {
        return _level == 0;
    }

    public bool Add(float amount)
    {
        if (amount < 0 || IsFull())
        {
            return false;
        }

        _level = Math.Min(1, _level + amount);
        
        return true;
    }

    public bool Drop(float amount)
    {
        if (amount < 0 || IsEmpty())
        {
            return false;
        }

        _level = Math.Max(0, _level - amount);
        
        return true;
    }
}
