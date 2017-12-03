using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport
{
    public GeoPoint Location { get; private set; }
    public string Name { get; private set; }

    public bool IsUnlocked { get; private set; }

    public float PassengerCount { get; internal set; }

    public Airport(AirportDefinition def)
    {
        Location = def.Location;
        Name = def.Name;
        IsUnlocked = def.UnlockedByDefault;
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}
