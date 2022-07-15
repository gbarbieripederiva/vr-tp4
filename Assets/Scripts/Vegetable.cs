using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    static public float lerpYSpeed = 0.1f;
    
    // From 0-x. 1.0f equals a whole watering can.
    public float waterNeeded;
    public Vector3 minScale;
    public GameObject initObject;
    [CanBeNull] public GameObject finalObject;
    public float initY;
    public float finalYDuringGrowth;
    public float finalY;
    public int rewardWhenGrown;

    private bool _isPlanted;
    private bool _hasBeenPlanted;
    private float _maxWater;
    private float _currentY;

    void Start()
    {
        if (finalObject != null)
        {
            initObject.SetActive(true);
            finalObject.SetActive(false);
        }
    }

    public bool IsPlanted()
    {
        return _isPlanted;
    }

    public bool HasBeenPlanted()
    {
        return _hasBeenPlanted;
    }

    public int GetReward()
    {
        if (!HasGrown())
        {
            return 1;
        }

        return rewardWhenGrown;
    }

    public void Plant(Plantable plantable)
    {
        plantable.SetPlant(this);
        _maxWater = waterNeeded;
        _setY();
        _hasBeenPlanted = _isPlanted = true;
    }

    public void UnPlant()
    {
        _isPlanted = false;
    }

    private void Update()
    {
        if (!IsPlanted())
        {
            return;
        }
        
        if (HasGrown() && Math.Abs(_currentY - finalY) > 0.0001f)
        {
            var transform1 = transform;
            var localPosition = transform1.localPosition;

            var finalPosition = Vector3.MoveTowards(localPosition, new Vector3(localPosition.x, finalY, localPosition.z), Time.deltaTime * lerpYSpeed);
            transform1.localPosition = finalPosition;
            _currentY = finalY;
        }
    }

    public bool HasGrown()
    {
        return waterNeeded == 0;
    }

    public bool AddWater(float amount)
    {
        if (HasGrown())
        {
            return false;
        }

        if (amount == 0)
        {
            return true;
        }

        waterNeeded = Math.Min(0, waterNeeded - amount);
        if (HasGrown())
        {
            _finalStage();
            return false;
        }
        _scale();
        
        return true;
    }

    private void _finalStage()
    {
        var transform1 = transform;
        var localPosition = transform1.localPosition;

        transform1.localScale = new Vector3(1, 1, 1);
        _currentY = localPosition.y;

        if (finalObject != null)
        {
            transform1.localPosition = new Vector3(localPosition.x, finalY, localPosition.z);
            _currentY = finalY;
            
            finalObject.SetActive(true);
            initObject.SetActive(false);
        }
    }

    private void _scale()
    {
        var complement = _maxWater - waterNeeded;
        var factor = complement / _maxWater;
        
        var scaleX = Utils.mapNumber(factor, 0, 1, minScale.x,  1);
        var scaleY = Utils.mapNumber(factor, 0, 1, minScale.y,  1);
        var scaleZ = Utils.mapNumber(factor, 0, 1, minScale.z,  1);

        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    private void _setY()
    {
        var complement = _maxWater - waterNeeded;
        var factor = complement / _maxWater;
        
        var transform1 = transform;
        var localPosition = transform1.localPosition;

        var y = Utils.mapNumber(factor, 0, 1, initY, finalYDuringGrowth);
        transform1.localPosition = new Vector3(localPosition.x, y, localPosition.z);
    }
}
