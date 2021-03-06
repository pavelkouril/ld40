﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloopManager : MonoBehaviour
{
    private bool _gameOver;

    public float Score { get; set; }

    private AirportManager _airportManager;
    private PlaneManager _planeManager;
    private UiManager _uiManager;
    private DaytimeManager _daytimeManager;
    private PassengerManager _passengerManager;
    private UpgradeManager _upgradeManager;

    private float airportsSpawnRate = DaytimeManager.kRealSecondsInDay * 3;
    private float passengerSpawnRate = DaytimeManager.kRealSecondsInDay / 1.5f;
    private float upgradesRate = DaytimeManager.kRealSecondsInDay * 3;
    private float planeRate = DaytimeManager.kRealSecondsInDay * 4.5f;

    private void Awake()
    {
        _airportManager = GetComponent<AirportManager>();
        _planeManager = GetComponent<PlaneManager>();
        _uiManager = GetComponent<UiManager>();
        _daytimeManager = GetComponent<DaytimeManager>();
        _passengerManager = GetComponent<PassengerManager>();
        _upgradeManager = GetComponent<UpgradeManager>();
    }

    private void Start()
    {
        StartCoroutine(UnlockFirstAirport());
        StartCoroutine(SpawnFirstPassengers());
        StartCoroutine(SpawnFirstPassengers());
        StartCoroutine(Upgrades());
        StartCoroutine(Planes());
    }

    private IEnumerator Planes()
    {
        while (true)
        {
            yield return new WaitForSeconds(planeRate);
            planeRate *= 0.925f;
            _uiManager.AddPlaneListItem(_planeManager.AddPlane());
        }
    }

    private IEnumerator Upgrades()
    {
        while (true)
        {
            yield return new WaitForSeconds(upgradesRate);
            upgradesRate *= 1.03f;
            _upgradeManager.Receive();
            _uiManager.UpdateUpgrades();
        }
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
            yield return new WaitForSeconds(passengerSpawnRate);
            passengerSpawnRate *= 0.97f;
            _airportManager.SpawnAtAllUnlockedAiports();
        }
    }

    private IEnumerator UnlockFirstAirport()
    {
        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 6);
        var firstAirport = _airportManager.GetRandomAirportCloseTo(_airportManager.startingAirport);
        UnlockAirport(firstAirport);

        yield return new WaitForSeconds(DaytimeManager.kRealSecondsInDay / 2);
        UnlockAirport(_airportManager.GetRandomAirportCloseTo(firstAirport));

        StartCoroutine(StartAirportSpawns());
    }

    private IEnumerator StartAirportSpawns()
    {
        yield return new WaitForSeconds(airportsSpawnRate);
        airportsSpawnRate *= 0.97f;
        Airport airport;
        while ((airport = _airportManager.GetRandomAirport()) != null)
        {
            UnlockAirport(airport);
            airportsSpawnRate *= 0.92f;
            yield return new WaitForSeconds(airportsSpawnRate);
        }
    }

    private void UnlockAirport(Airport airport)
    {
        if (_airportManager.UnlockAirport(airport))
        {
            _uiManager.NotifyNewAirport(airport);
            var plane = _planeManager.AddPlane();
            _uiManager.AddPlaneListItem(plane);
        }
    }

    private void Update()
    {
        foreach (var airport in _airportManager.ActiveAirports)
        {
            if (airport.PassengerCount > 1.33f && !_gameOver)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        _gameOver = true;
        Score = _daytimeManager.TotalSeconds;
        _uiManager.Gameover();
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
