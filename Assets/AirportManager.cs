using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportManager : MonoBehaviour
{
    [SerializeField]
    private List<AirportDefinition> _airportDefinitions = new List<AirportDefinition>();

    [SerializeField]
    private AirportGlobeVisualisation _globeVisPrefab;

    private Dictionary<GeoPoint, Airport> _airports = new Dictionary<GeoPoint, Airport>();

    private void Awake()
    {
        foreach (var def in _airportDefinitions)
        {
            _airports.Add(def.Location, new Airport(def));
            var globeVis = Instantiate(_globeVisPrefab);
            globeVis.Location = def.Location;
            Debug.Log(def.Location);
        }
    }
}
