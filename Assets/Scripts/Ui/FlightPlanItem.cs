using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightPlanItem : MonoBehaviour
{
    public Airport Airport
    {
        get
        {
            return _airport;
        }

        set
        {
            _airport = value;
            _airportName.text = value.Name;
        }
    }

    [SerializeField]
    private Text _airportName;

    private Airport _airport;
}
