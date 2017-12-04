using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string Name { get; set; }

    public enum Upgrades
    {
        None,
        Level1,
        Level2,
    };

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

    public Upgrades CurrentUpgrade
    {
        get
        {
            return _currentUpgrade;
        }
    }

    public int MaxStops
    {
        get
        {
            if (_currentUpgrade == Upgrades.Level1)
            {
                return 3;
            }
            if (_currentUpgrade == Upgrades.Level2)
            {
                return 5;
            }
            return 2;
        }
    }

    public float Speed = 300.0f;

    public bool IsDispatched { get; private set; }
    public Airport TargetAirport
    {
        get
        {
            return _currentTarget;
        }
    }

    private List<Airport> _flightPlan = new List<Airport>();

    private Queue<Airport> _airportQueue = new Queue<Airport>();

    private Airport _previousTarget;
    private Airport _currentTarget;
    private RoutePath _currentRoute;

    private Upgrades _currentUpgrade;

    private bool _awaitingNewPlan;

    private void Update()
    {
        if (IsDispatched && _currentRoute != null)
        {
            // move the airplane, call the arrive at target logic when necessary, etc.
            Quaternion q = new Quaternion();
            Vector3 position = _currentRoute.Update(DaytimeManager.DeltaTimeMs, ref q);
            transform.localPosition = position;
            transform.localRotation = q;

            if (_currentRoute.Finished())
            {
                ArriveAtTarget();
            }
        }
    }

    private void ArriveAtTarget()
    {
        _previousTarget = _currentTarget;
        _currentRoute = null;

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
        _currentTarget.BoardPassengers();
        _currentTarget = _airportQueue.Dequeue();
        _currentRoute = new RoutePath(_previousTarget.Location, _currentTarget.Location, Speed);
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

    /// <summary>
    /// Dispatches the plane on a flight (if not dispatched already)
    /// </summary>
    public void Dispatch()
    {
        if (!IsDispatched)
        {
            IsDispatched = true;
            FillQueue();
            _previousTarget = _flightPlan[0];
            _currentTarget = _airportQueue.Dequeue();
            transform.position = _flightPlan[0].Location.ToSphericalCartesian();
            _currentRoute = new RoutePath(_previousTarget.Location, _currentTarget.Location, Speed);
            gameObject.SetActive(true);
        }
    }

    public bool Upgrade()
    {
        if (_currentUpgrade != Upgrades.Level2)
        {
            _currentUpgrade = _currentUpgrade + 1;
            return true;
        }
        return false;
    }
}
