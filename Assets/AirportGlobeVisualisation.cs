using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportGlobeVisualisation : MonoBehaviour
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

    [SerializeField]
    private float _radius = 1;

    private void Update()
    {
        transform.position = new Vector3(
              _radius * Mathf.Cos(_location.Latitude * Mathf.Deg2Rad) * Mathf.Cos(_location.Longitude * Mathf.Deg2Rad),
              _radius * Mathf.Cos(_location.Latitude * Mathf.Deg2Rad) * Mathf.Sin(_location.Longitude * Mathf.Deg2Rad),
              _radius * Mathf.Sin(_location.Latitude * Mathf.Deg2Rad));
    }
}
