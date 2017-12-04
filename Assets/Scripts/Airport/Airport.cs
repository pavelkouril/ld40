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

    public void BoardPassengers()
    {
        PassengerCount *= Random.Range(0.7f, 0.95f);
    }

    public void SpawnPassengers()
    {
        PassengerCount += Random.Range(0.04f, 0.09f);
    }
}
