using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AirportManager : MonoBehaviour
{
    [SerializeField]
    private List<AirportDefinition> _airportDefinitions = new List<AirportDefinition>();

    [SerializeField]
    private AirportVisualisation _globeVisPrefab;

    [SerializeField]
    private AirportVisualisation _mapVisPrefab;

    [SerializeField]
    private Transform _globeVisParent;

    private Dictionary<GeoPoint, Airport> _airports = new Dictionary<GeoPoint, Airport>();

    private ResourceManager _resourceManager;

    public Airport startingAirport;

    private void Awake()
    {
        _resourceManager = GetComponent<ResourceManager>();
        foreach (var def in _airportDefinitions)
        {
            var airport = new Airport(def);
            _airports.Add(def.Location, airport);
            if (def.UnlockedByDefault)
            {
                startingAirport = airport;
            }

            var globeVis = Instantiate(_globeVisPrefab, _globeVisParent.position + def.Location.ToSphericalCartesian(), Quaternion.identity, _globeVisParent);
            globeVis.Airport = airport;            
        }
    }

    public bool UnlockAirport(Airport airport)
    {
        airport.Unlock();
        return true;
    }

    internal void SpawnAtAllUnlockedAiports()
    {
        foreach (var a in _airports.Values)
        {
            if (a.IsUnlocked)
            {
                a.SpawnPassengers();
            }
        }
    }

    public Airport GetRandomAirportCloseTo(Airport airport)
    {
        return _airports.Values.Where(a => !a.IsUnlocked).OrderBy(a => GeoPoint.Distance(a.Location, airport.Location)).Take(5).OrderBy(qu => Guid.NewGuid()).First();
    }

    public Airport GetRandomAirport()
    {
        return _airports.Values.Where(a => !a.IsUnlocked).OrderBy(qu => Guid.NewGuid()).First();
    }
}
