using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
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
    private int _remainingPlanes;

    [SerializeField]
    private int _remainingUpgrades;

    public void AwardPlane()
    {
        _remainingPlanes++;
    }

    public void AwardUpgrade()
    {
        _remainingUpgrades++;
    }

    public bool UsePlane()
    {
        if (_remainingPlanes > 0)
        {
            _remainingPlanes--;
            return true;
        }
        return false;

    }

    public bool UseUpgrade()
    {
        if (_remainingUpgrades > 0)
        {
            _remainingUpgrades--;
            return true;
        }
        return false;
    }
}
