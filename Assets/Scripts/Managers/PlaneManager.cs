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
    private Plane _planePrefab;

    [SerializeField]
    private List<string> _names = new List<string>();

    private List<Plane> _planes = new List<Plane>();

    private ResourceManager _resourceManager;

    private void Awake()
    {
        _resourceManager = GetComponent<ResourceManager>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public Plane AddPlane()
    {
        return AddPlane(false);
    }

    public Plane AddPlane(bool gratis)
    {
        if (gratis || _resourceManager.UsePlane())
        {
            var plane = Instantiate(_planePrefab);
            plane.Name = RandomName();
            plane.gameObject.SetActive(false);
            _planes.Add(plane);
            return plane;
        }
        return null;
    }

    private string RandomName()
    {
        var name = _names[UnityEngine.Random.Range(0, _names.Count)];
        _names.Remove(name);
        return name;
    }

    public bool UpgradePlane()
    {
        return false;
    }
}
