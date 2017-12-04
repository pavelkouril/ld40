using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public List<Plane> Planes
    {
        get
        {
            return _planes;
        }
    }

    [SerializeField]
    private Transform _globe;

    [SerializeField]
    private Plane _planePrefab;

    [SerializeField]
    private List<string> _names = new List<string>();

    private List<Plane> _planes = new List<Plane>();

    private UpgradeManager _resourceManager;

    private void Awake()
    {
        _resourceManager = GetComponent<UpgradeManager>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }


    public Plane AddPlane()
    {
        var plane = Instantiate(_planePrefab, _globe);
        plane.Name = RandomName();
        plane.gameObject.SetActive(false);
        _planes.Add(plane);
        return plane;
    }

    private string RandomName()
    {
        if (_names.Count > 0)
        {
            var name = _names[UnityEngine.Random.Range(0, _names.Count)];
            _names.Remove(name);
            return name;
        }
        else
        {
            return "Unknown";
        }
    }

    public bool UpgradePlane()
    {
        return false;
    }
}
