using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int RemainingAirports
    {
        get
        {
            return _remainingAirports;
        }
    }

    public int RemainingPlanes
    {
        get
        {
            return _remainingPlanes;
        }
    }

    public int RemainingUpgrades
    {
        get
        {
            return _remainingUpgrades;
        }
    }

    [SerializeField]
    private int _remainingAirports;

    [SerializeField]
    private int _remainingPlanes;

    [SerializeField]
    private int _remainingUpgrades;

    public void AwardAirport()
    {
        _remainingAirports++;
    }

    public void AwardPlane()
    {
        _remainingPlanes++;
    }

    public void AwardUpgrade()
    {
        _remainingUpgrades++;
    }

    public bool UseAirport()
    {
        if (_remainingAirports > 0)
        {
            _remainingAirports--;
            return true;
        }
        return false;
    }

    public bool UsePlane()
    {
        if (_remainingAirports > 0)
        {
            _remainingPlanes--;
            return true;
        }
        return false;

    }

    public bool UseUpgrade()
    {
        if (_remainingAirports > 0)
        {
            _remainingUpgrades--;
            return true;
        }
        return false;
    }
}
