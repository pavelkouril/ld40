using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string Name { get; set; }

    public bool HasTarget
    {
        get
        {
            return _currentTarget != null;
        }
    }

    public List<Airport> FlightPlan
    {
        get
        {
            return _flightPlan;
        }
    }

    private List<Airport> _flightPlan = new List<Airport>();

    private Queue<Airport> _airportQueue = new Queue<Airport>();

    private Airport _currentTarget;

    private bool _awaitingNewPlan;

    private void AriveAtTarget()
    {
        if (_currentTarget == null)
        {
            Debug.Log("Can't arrive at null.");
            return;
        }
        if (_airportQueue.Count == 0)
        {
            FillQueue();
        }
        StartCoroutine(WaitForPassengers(1));
    }

    private IEnumerator WaitForPassengers(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _currentTarget = _airportQueue.Dequeue();
    }

    public void SetNewPlan(List<Airport> tempFlightPlan)
    {
        _awaitingNewPlan = true;
        _flightPlan.Clear();
        foreach (var airport in tempFlightPlan)
        {
            _flightPlan.Add(airport);
        }
    }

    private void FillQueue()
    {
        for (var i = 0; i < _flightPlan.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }
            _airportQueue.Enqueue(_flightPlan[i]);
        }
        _airportQueue.Enqueue(_flightPlan[0]);
    }
}
