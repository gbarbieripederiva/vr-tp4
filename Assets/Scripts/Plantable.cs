using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantable : MonoBehaviour
{
    private Vegetable _plant;
    
    public bool IsOccupied()
    {
        return _plant == null;
    }

    public void SetPlant(Vegetable plant)
    {
        _plant = plant;
        plant.gameObject.transform.position = transform.position;
    }

    public Vegetable RemovePlant()
    {
        var aux = _plant;
        aux.UnPlant();
        _plant = null;
        return _plant;
    }
}
