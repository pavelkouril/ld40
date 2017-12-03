using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloopManager : MonoBehaviour
{
    private AirportManager _airportManager;
    private PlaneManager _planeManager;
    private UiManager _uiManager;
    private DaytimeManager _daytimeManager;
    private PassengerManager _passengerManager;
    private float airportsSpawnRate;

    private void Awake()
    {
        _airportManager = GetComponent<AirportManager>();
        _planeManager = GetComponent<PlaneManager>();
        _uiManager = GetComponent<UiManager>();
        _daytimeManager = GetComponent<DaytimeManager>();
        _passengerManager = GetComponent<PassengerManager>();
    }

    private void Start()
    {
        StartCoroutine(UnlockFirstAirport());
        StartCoroutine(SpawnFirstPassengers());
    }

    private IEnumerator SpawnFirstPassengers()
    {
        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 5);
        _airportManager.startingAirport.SpawnPassengers();
        StartCoroutine(SpawnPassengers());
    }

    private IEnumerator SpawnPassengers()
    {
        while (true)
        {
            yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 4);
            _airportManager.SpawnAtAllUnlockedAiports();
        }
    }

    private IEnumerator UnlockFirstAirport()
    {
        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 3);
        var airport = _airportManager.GetRandomAirportCloseTo(_airportManager.startingAirport);
        if (_airportManager.UnlockAirport(airport))
        {
            var plane = _planeManager.AddPlane(true);
            _uiManager.AddPlaneListItem(plane);
        }

        StartCoroutine(StartAirportSpawns());
    }

    private IEnumerator StartAirportSpawns()
    {
        airportsSpawnRate = DaytimeManager.kRealSecondsInDay * 5;
        yield return new WaitForSeconds(airportsSpawnRate);
        airportsSpawnRate *= 0.92f;
        Airport airport;
        while ((airport = _airportManager.GetRandomAirport()) != null)
        {
            if (_airportManager.UnlockAirport(airport))
            {
                var plane = _planeManager.AddPlane(true);
                _uiManager.AddPlaneListItem(plane);
            }
            airportsSpawnRate *= 0.92f;
            yield return new WaitForSeconds(airportsSpawnRate);
        }
    }

    private void Update()
    {

    }

    public void UnlockAirport()
    {

    }

    public void NewDay(float dayOfMonth, float month)
    {

    }

    public void NewMonth(float dayOfMonth, float month)
    {

    }
}
