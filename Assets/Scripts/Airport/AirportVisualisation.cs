using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportVisualisation : MonoBehaviour
{
    public GeoPoint Location
    {
        get
        {
            return _location;
        }

        set
        {
            _location = value;
        }
    }

    public Airport Airport { get; set; }

    [SerializeField]
    private Material _lockedMaterial;

    [SerializeField]
    private Material _unlockedMaterial;

    [SerializeField]
    private GeoPoint _location;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        _meshRenderer.enabled = Airport.IsUnlocked;
    }

    private void Update()
    {
        if (Airport.IsUnlocked)
        {
            if (!_meshRenderer.enabled)
            {
                _meshRenderer.enabled = true;
            }
            _meshRenderer.material = _unlockedMaterial;
            _meshRenderer.material.SetFloat("_Float0", Mathf.Clamp01(Airport.PassengerCount));
        }
    }
}
