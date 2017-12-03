using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport
{
    public GeoPoint Location { get; private set; }
    public string Name { get; private set; }

    public bool IsUnlocked { get; private set; }

    public Airport(AirportDefinition def)
    {
        Location = def.Location;
        Name = def.Name;
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}
