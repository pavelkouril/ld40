using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportManager : MonoBehaviour
{
    [SerializeField]
    private List<AirportDefinition> _airportDefinitions = new List<AirportDefinition>();

    private Dictionary<GeoPoint, Airport> _airports = new Dictionary<GeoPoint, Airport>();

    private void Awake()
    {
        // _airports.Add(new GeoPoint(62.464106f, -125.068359f), new Airport());
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
