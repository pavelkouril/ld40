using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text _daytimeText;

    [SerializeField]
    private GameObject _textPanel;

    [SerializeField]
    private GameObject _airportPanel;

    [SerializeField]
    private Color _uiBgColor;

    private Camera _camera;

    private DaytimeManager _daytimeManager;
    private AirportManager _airportManager;

    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;

    private Airport _currentAirport;

    private void Awake()
    {
        _daytimeManager = GetComponent<DaytimeManager>();
        _airportManager = GetComponent<AirportManager>();
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

        // setup all default bg colors
        var transparentDefault = _uiBgColor;
        transparentDefault.a = 0;
        _airportPanel.GetComponent<Image>().color = transparentDefault;
        _textPanel.GetComponent<Image>().color = transparentDefault;

        // hide all panels
        _airportPanel.SetActive(false);
        _textPanel.SetActive(false);
    }

    private void Update()
    {
        _daytimeText.text = string.Format("Time: {0}\nDay:{1}\nMonth:{2}", _daytimeManager.TimeOfDayUtc, _daytimeManager.DayOfMonth, _daytimeManager.Month);
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 9))
            {
                ShowAirportPanel(hit.collider.GetComponent<AirportVisualisation>().Airport);
            }
        }
    }

    public void ToggleMode()
    {
        if (CurrentMode == DisplayModes.Globe)
        {
            CurrentMode = DisplayModes.Map;
            _camera.GetComponent<CameraController>().state = CameraState.ORBIT_TO_PLANE;
            ShowMap();
            StoreCamera();
        }
        else
        {
            CurrentMode = DisplayModes.Globe;
            _camera.GetComponent<CameraController>().state = CameraState.PLANE_TO_ORBIT;
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

    private void ShowAirportPanel(Airport airport)
    {
        _currentAirport = airport;
        _airportPanel.SetActive(true);
        LeanTween.alpha(_airportPanel.GetComponent<RectTransform>(), _uiBgColor.a, 1).setUseEstimatedTime(true).setOnComplete(() =>
        {
        });
    }

    private void HideAirportPanel()
    {
        LeanTween.alpha(_airportPanel.GetComponent<RectTransform>(), 0, 1).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _currentAirport = null;
            _airportPanel.SetActive(false);
        });
    }

    private void ShowTextPanel(string text)
    {
        _textPanel.SetActive(true);
        LeanTween.alpha(_textPanel.GetComponent<RectTransform>(), _uiBgColor.a, 1).setUseEstimatedTime(true).setOnComplete(() =>
        {
        });
    }

    private void HideTextPanel()
    {
        LeanTween.alpha(_textPanel.GetComponent<RectTransform>(), 0, 1).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _textPanel.SetActive(false);
        });
    }

    public void UnlockAirport()
    {
        if (_airportManager.UnlockAirport(_currentAirport))
        {
            Debug.Log("airport obtained. great success!");
            _currentAirport = null;
            HideAirportPanel();
        }
    }
}
