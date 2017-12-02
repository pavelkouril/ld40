using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportVisualisation : MonoBehaviour
{
    public GeoPoint Location
    {
        get
        {
            return _location;
        }

        set
        {
            _location = value;
        }
    }

    [SerializeField]
    private GeoPoint _location;

    private void Update()
    {

    }
}
