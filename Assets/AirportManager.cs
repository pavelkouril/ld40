using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private Transform _mapVisParent;

    private Dictionary<GeoPoint, Airport> _airports = new Dictionary<GeoPoint, Airport>();

    private ResourceManager _resourceManager;

    private void Awake()
    {
        foreach (var def in _airportDefinitions)
        {
            _airports.Add(def.Location, new Airport(def));

            var globeVis = Instantiate(_globeVisPrefab, _globeVisParent.position + def.Location.ToSphericalCartesian(), Quaternion.identity, _globeVisParent);

            var planarCartesians = def.Location.ToPlanarCartesian();
            var pos = _mapVisParent.position + new Vector3(0.05f, 1 * (planarCartesians.y * 2.0f - 1.0f), 2 * (planarCartesians.x * 2.0f - 1.0f));

            var mapVis = Instantiate(_mapVisPrefab, pos, Quaternion.Euler(0, 270, 270), _mapVisParent);
        }
    }

    public bool UnlockAirport(Airport airport)
    {
        if (_resourceManager.UseAirport())
        {
            airport.Unlock();
            return true;
        }
        return false;
    }
}
