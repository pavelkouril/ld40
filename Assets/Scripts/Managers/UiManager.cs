using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public const float kPanelFadeTime = 0.5f;

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

    #region Buttons

    [Header("Buttons")]
    [SerializeField]
    private Button _menuButton;

    [SerializeField]
    private Button _lockButton;

    [SerializeField]
    private Button _unlockButton;
    [SerializeField]
    private Button _modeButton;

    #endregion

    #region Colors

    [Header("Colors")]
    [ColorUsage(false)]
    [SerializeField]
    private Color _uiBgColor;

    [Range(0, 1)]
    [SerializeField]
    private float _targetAlpha;

    #endregion

    [Header("Text panel")]
    [SerializeField]
    private GameObject _textPanel;

    [SerializeField]
    private Text _textPanelContent;

    #region Planes

    [Header("Planes")]
    [SerializeField]
    private RectTransform _planeList;

    [SerializeField]
    private PlaneListItem _planeListItemPrefab;

    #endregion

    #region FlightPlanPanel

    [Header("Flight plan panel")]
    [SerializeField]
    private GameObject _flightPlanPanel;

    [SerializeField]
    private Text _flightPlanPanelName;

    [SerializeField]
    private Button _flightPlanSaveButton;

    [SerializeField]
    private FlightPlanItem _flightPlanItemPrefab;

    [SerializeField]
    private RectTransform _flightStopsList;

    #endregion

    private Camera _camera;
    private CameraController _cameraController;
    private DaytimeManager _daytimeManager;
    private AirportManager _airportManager;
    private PlaneManager _planeManager;

    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;

    private Airport _currentAirport;
    private Plane _currentPlane;

    private bool _lockAirportPanelOpen;
    private bool _selectAirportForPlan;

    private List<Airport> _tempFlightPlan = new List<Airport>();

    private bool _sameStartAndEnd;

    private void Awake()
    {
        _daytimeManager = GetComponent<DaytimeManager>();
        _planeManager = GetComponent<PlaneManager>();
        _airportManager = GetComponent<AirportManager>();
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        StoreCamera();
        _unlockButton.gameObject.SetActive(_cameraController.lockRotation);
        _lockButton.gameObject.SetActive(!_cameraController.lockRotation);
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
        _textPanel.GetComponent<Image>().color = transparentDefault;
        _flightPlanPanel.GetComponent<Image>().color = transparentDefault;

        // hide all panels
        _textPanel.SetActive(false);
        _flightPlanPanel.SetActive(false);
    }

    private void Update()
    {
        _daytimeText.text = string.Format("Time: {0}\nDay:{1}\nMonth:{2}", _daytimeManager.TimeOfDayUtc, _daytimeManager.DayOfMonth, _daytimeManager.Month);

        // handle opening airport panels
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 9))
            {
                var airport = hit.collider.GetComponent<AirportVisualisation>().Airport;
                if (!_lockAirportPanelOpen && !airport.IsUnlocked)
                {
                    
                }
                else if (_selectAirportForPlan && airport.IsUnlocked)
                {
                    AddFlightPlanItem(airport);
                }
            }
        }

        _flightPlanSaveButton.interactable = !_sameStartAndEnd && _tempFlightPlan.Count >= 2;
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

  

    private void ShowTextPanel(string text)
    {
        _textPanel.SetActive(true);
        LeanTween.alpha(_textPanel.GetComponent<RectTransform>(), _targetAlpha, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
        });
    }

    private void HideTextPanel()
    {
        LeanTween.alpha(_textPanel.GetComponent<RectTransform>(), 0, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _textPanel.SetActive(false);
        });
    }

    public void AddPlaneListItem(Plane plane)
    {
        var listItem = Instantiate(_planeListItemPrefab, _planeList);
        listItem.Plane = plane;
        listItem.FlightPlanButton.onClick.AddListener(() => ShowFlightPlanPanel(plane));
    }

    private void ShowFlightPlanPanel(Plane plane)
    {
        _lockAirportPanelOpen = true;
        _currentPlane = plane;
        SetFlightPanelItems(plane);
        _flightPlanPanelName.text = plane.Name;
        _flightPlanPanel.SetActive(true);
        LeanTween.alpha(_flightPlanPanel.GetComponent<RectTransform>(), _targetAlpha, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _selectAirportForPlan = true;
        });
    }

    public void HideFlightPlanPanel()
    {
        HideFlightPlanPanel(null);
    }

    public void HideFlightPlanPanel(Plane plane)
    {
        LeanTween.alpha(_flightPlanPanel.GetComponent<RectTransform>(), 0, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _selectAirportForPlan = false;
            _lockAirportPanelOpen = false;
            _currentPlane = null;
            _flightPlanPanel.SetActive(false);
            if (plane != null)
            {
                ShowFlightPlanPanel(plane);
            }
        });
    }

    private void AddFlightPlanItem(Airport airport)
    {
        if (airport != _tempFlightPlan.LastOrDefault())
        {
            var item = Instantiate(_flightPlanItemPrefab, _flightStopsList);
            item.Airport = airport;
            _tempFlightPlan.Add(airport);
            _sameStartAndEnd = airport == _tempFlightPlan.FirstOrDefault();
        }
        else
        {
            Debug.Log("Can't have same end points.");
        }
    }

    private void SetFlightPanelItems(Plane plane)
    {
        _tempFlightPlan.Clear();
        foreach (var item in _flightStopsList.GetComponentsInChildren<FlightPlanItem>())
        {
            Destroy(item.gameObject);
        }
        foreach (var stop in plane.FlightPlan)
        {
            AddFlightPlanItem(stop);
        }
    }

    public void SaveFlightPlan()
    {
        _currentPlane.SetNewPlan(_tempFlightPlan);
        _currentPlane.Dispatch();
        HideFlightPlanPanel();
    }

    public void ToggleCameraLock()
    {
        _cameraController.lockRotation = !_cameraController.lockRotation;
        _unlockButton.gameObject.SetActive(_cameraController.lockRotation);
        _lockButton.gameObject.SetActive(!_cameraController.lockRotation);
    }
}
