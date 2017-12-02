using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public enum DisplayModes
    {
        Globe,
        Map,
    }

    public DisplayModes CurrentMode { get; private set; }

    [SerializeField]
    private GameObject _globe;

    [SerializeField]
    private GameObject _map;

    private Camera _camera;

    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;

    private void Awake()
    {
        _camera = Camera.main;
        StoreCamera();
    }

    private void Start()
    {
        if (CurrentMode == DisplayModes.Globe)
        {
            ShowGlobe();
        }
        else
        {
            ShowMap();
        }
    }

    private void Update()
    {

    }

    public void ToggleMode()
    {
        if (CurrentMode == DisplayModes.Globe)
        {
            CurrentMode = DisplayModes.Map;
            ShowMap();
            StoreCamera();
        }
        else
        {
            CurrentMode = DisplayModes.Globe;
            ShowGlobe();
            RestoreCamera();
        }
    }

    private void ShowGlobe()
    {
        _map.SetActive(false);
        _globe.SetActive(true);
        _camera.transform.parent = null;
    }

    private void ShowMap()
    {
        _globe.SetActive(false);
        _map.SetActive(true);
        _camera.transform.parent = _map.transform;
        _camera.transform.localPosition = new Vector3(3f, 0, 0);
        _camera.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    private void StoreCamera()
    {
        oldCameraPos = _camera.transform.localPosition;
        oldCameraRot = _camera.transform.localRotation;
    }

    private void RestoreCamera()
    {
        _camera.transform.localPosition = oldCameraPos;
        _camera.transform.localRotation = oldCameraRot;
    }
}
