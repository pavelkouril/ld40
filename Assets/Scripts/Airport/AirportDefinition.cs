using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AirportDefinition
{
    public GeoPoint Location
    {
        get
        {
            return _location;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }

    public bool UnlockedByDefault
    {
        get
        {
            return _unlockedByDefault;
        }
    }

    [SerializeField]
    private GeoPoint _location;

    [SerializeField]
    private string _name;

    [SerializeField]
    private bool _unlockedByDefault;
}
