using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int RemainingUpgrades
    {
        get
        {
            return _remainingUpgrades;
        }
    }

    [SerializeField]
    private int _remainingUpgrades;

    public void Receive()
    {
        _remainingUpgrades++;
    }

    public bool Use(Plane plane)
    {
        if (_remainingUpgrades > 0)
        {
            _remainingUpgrades--;
            plane.Upgrade();
            return true;
        }
        return false;
    }
}
