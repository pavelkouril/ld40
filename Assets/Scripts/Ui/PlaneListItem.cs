using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneListItem : MonoBehaviour
{
    public Plane Plane
    {
        get
        {
            return _plane;
        }
        set
        {
            _plane = value;
            _name.text = _plane.Name;
        }
    }

    public Button FlightPlanButton
    {
        get
        {
            return _flightPlanButton;
        }
    }

    public Button UpgradeButton
    {
        get
        {
            return _upgradeButton;
        }
    }

    [SerializeField]
    private Text _name;

    [SerializeField]
    private Button _flightPlanButton;

    [SerializeField]
    private Button _upgradeButton;

    private Plane _plane;

    private void Start()
    {

    }

    private void Update()
    {

    }
}
