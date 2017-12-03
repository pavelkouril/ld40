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
        StartCoroutine(PopPassenger());
    }

    private IEnumerator PopPassenger()
    {
        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 5);
        _passengerManager.SpawnAtAirport(_airportManager.startingAirport);
    }

    private IEnumerator UnlockFirstAirport()
    {
        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 2);
        var airport = _airportManager.GetRandomAirportCloseTo(_airportManager.startingAirport);
        if (_airportManager.UnlockAirport(airport))
        {
            var plane = _planeManager.AddPlane(true);
            _uiManager.AddPlaneListItem(plane);
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
