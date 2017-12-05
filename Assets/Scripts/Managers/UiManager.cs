using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

    [SerializeField]
    private Image _preserveAlphaPanel;

    [SerializeField]
    private Text _dateText;

    [SerializeField]
    private Text _upgradesText;

    [SerializeField]
    private GameObject _gameMenu;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _gameOverMenu;

    [SerializeField]
    private AirportNotification _airportNotification;


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

    #region Gameover

    [Header("Game over")]
    [SerializeField]
    private Text _gameOverScore;

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

    [SerializeField]
    private Text _stopsCount;

    #endregion

    private Camera _camera;
    private CameraController _cameraController;
    private DaytimeManager _daytimeManager;
    private AirportManager _airportManager;
    private PlaneManager _planeManager;
    private UpgradeManager _upgradeManager;
    private GameloopManager _gameloopManager;

    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;

    private Airport _currentAirport;
    private Plane _currentPlane;

    private bool _lockAirportPanelOpen;
    private bool _selectAirportForPlan;

    private List<Airport> _tempFlightPlan = new List<Airport>();

    private bool _sameStartAndEnd;

    private Color _preserveColor;

    [SerializeField]
    private int _pathTessellation;
    [SerializeField]
    private Material _pathMaterial;
    private PathEffect _pathEffect;

    [SerializeField]
    private InputField _inputField;
    public int _score;
    private UnityWebRequest _www;
    private Airport _notifiedAirport;

    private void Awake()
    {
        _gameloopManager = GetComponent<GameloopManager>();
        _daytimeManager = GetComponent<DaytimeManager>();
        _planeManager = GetComponent<PlaneManager>();
        _airportManager = GetComponent<AirportManager>();
        _upgradeManager = GetComponent<UpgradeManager>();
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        StoreCamera();
        _unlockButton.gameObject.SetActive(_cameraController.lockRotation);
        _lockButton.gameObject.SetActive(!_cameraController.lockRotation);
        _pathEffect = new PathEffect(_pathTessellation);
        _preserveColor = _preserveAlphaPanel.color;
    }

    internal void NotifyNewAirport(Airport airport)
    {
        _notifiedAirport = airport;
        _airportNotification.gameObject.SetActive(true);
    }

    public void ClickAirportNotification()
    {
        if (_notifiedAirport != null)
        {
            _airportNotification.gameObject.SetActive(false);
            _cameraController.GotoAirport(_notifiedAirport);
        }
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
        _pauseMenu.GetComponent<Image>().color = transparentDefault;
        _gameOverMenu.GetComponent<Image>().color = transparentDefault;

        // hide all panels
        _textPanel.SetActive(false);
        _flightPlanPanel.SetActive(false);
        _pauseMenu.SetActive(false);
        _gameMenu.SetActive(true);
        _gameOverMenu.SetActive(false);

        UpdateUpgrades();

        _airportNotification.gameObject.SetActive(false);
    }

    private void Update()
    {
        // todo proper minutes
        _daytimeText.text = string.Format("{0:00}:{1:00} UTC", 24f * _daytimeManager.TimeOfDayUtc, 0);
        _dateText.text = string.Format("{0:00}/{1:00}", _daytimeManager.DayOfMonth, _daytimeManager.Month);

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
        if (_currentPlane)
        {
            _stopsCount.text = _tempFlightPlan.Count + "/" + _currentPlane.MaxStops;
        }

        _pathEffect.Update(Time.deltaTime);
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
        listItem.UpgradeButton.onClick.AddListener(() => UpgradePlane(plane));
        listItem.UpgradeButton.enabled = _upgradeManager.RemainingUpgrades > 0;
    }

    private void ShowFlightPlanPanel(Plane plane)
    {
        _pathEffect.Enable(_globe.gameObject, _pathMaterial);

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

        _pathEffect.Disable();
    }

    private void AddFlightPlanItem(Airport airport)
    {
        if (airport != _tempFlightPlan.LastOrDefault() && _tempFlightPlan.Count < _currentPlane.MaxStops)
        {
            var item = Instantiate(_flightPlanItemPrefab, _flightStopsList);
            item.Airport = airport;
            _tempFlightPlan.Add(airport);
            _sameStartAndEnd = airport == _tempFlightPlan.FirstOrDefault();

            _pathEffect.Clear();
            for (int i = 0; i < _tempFlightPlan.Count - 1; i++)
            {
                _pathEffect.AddRoute(_tempFlightPlan[i].Location, _tempFlightPlan[i + 1].Location);
            }

            if (_tempFlightPlan.Count > 2)
            {
                _pathEffect.AddRoute(_tempFlightPlan[_tempFlightPlan.Count - 1].Location, _tempFlightPlan[0].Location);
            }
        }
    }

    private void SetFlightPanelItems(Plane plane)
    {
        ClearFlightPlan();
        foreach (var stop in plane.FlightPlan)
        {
            AddFlightPlanItem(stop);
        }
    }

    public void ClearFlightPlan()
    {
        _tempFlightPlan.Clear();
        foreach (var item in _flightStopsList.GetComponentsInChildren<FlightPlanItem>())
        {
            Destroy(item.gameObject);
        }

        _pathEffect.Clear();
    }

    public void SaveFlightPlan()
    {
        _currentPlane.SetNewPlan(_tempFlightPlan);
        _currentPlane.Dispatch();
        HideFlightPlanPanel();
    }

    public void ToggleCameraLock()
    {
        //_cameraController.lockRotation = !_cameraController.lockRotation;
        //_unlockButton.gameObject.SetActive(_cameraController.lockRotation);
        //_lockButton.gameObject.SetActive(!_cameraController.lockRotation);

        bool locked = _cameraController.lockRotation;

        if (locked)
        {
            _cameraController.Unlock();
            _unlockButton.gameObject.SetActive(_cameraController.lockRotation);
            _lockButton.gameObject.SetActive(!_cameraController.lockRotation);
        }
        else
        {
            _cameraController.Lock();
            _unlockButton.gameObject.SetActive(_cameraController.lockRotation);
            _lockButton.gameObject.SetActive(!_cameraController.lockRotation);
        }
    }

    public void UpgradePlane(Plane plane)
    {
        _upgradeManager.Use(plane);
        UpdateUpgrades();
    }

    public void UpdateUpgrades()
    {
        foreach (var item in _planeList.GetComponentsInChildren<PlaneListItem>())
        {
            item.UpgradeButton.gameObject.SetActive(_upgradeManager.RemainingUpgrades > 0);
        }
        _upgradesText.text = _upgradeManager.RemainingUpgrades + " Upgrades";
    }

    public void Gameover()
    {
        _gameOverScore.text = ((int)_gameloopManager.Score).ToString();
        LeanTween.alpha(_gameMenu.GetComponent<RectTransform>(), 0, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _selectAirportForPlan = false;
            _lockAirportPanelOpen = false;
            _gameOverMenu.SetActive(true);
            _gameMenu.SetActive(false);
            LeanTween.alpha(_gameOverMenu.GetComponent<RectTransform>(), _targetAlpha, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
            {
            });
        });
    }

    public void PauseGame()
    {
        _preserveAlphaPanel.color = _preserveColor;
        Time.timeScale = 0;
        LeanTween.alpha(_gameMenu.GetComponent<RectTransform>(), 0, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _preserveAlphaPanel.color = _preserveColor;
            _pauseMenu.SetActive(true);
            _gameMenu.SetActive(false);
            LeanTween.alpha(_pauseMenu.GetComponent<RectTransform>(), _targetAlpha, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
            {
            });
        });
    }

    public void ResumeGame()
    {
        _preserveAlphaPanel.color = _preserveColor;
        _gameMenu.SetActive(true);
        LeanTween.alpha(_pauseMenu.GetComponent<RectTransform>(), 0, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
        {
            _preserveAlphaPanel.color = _preserveColor;
            _pauseMenu.SetActive(false);
            LeanTween.alpha(_gameMenu.GetComponent<RectTransform>(), 1, kPanelFadeTime).setUseEstimatedTime(true).setOnComplete(() =>
            {
                Time.timeScale = 1;
                _preserveAlphaPanel.color = _preserveColor;
            });
        });

    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", _inputField.text.Replace("|", " ").ToString());
        form.AddField("score", (int)_gameloopManager.Score);
        _www = UnityWebRequest.Post("https://otte.cz/ld40/index.php", form);
        yield return _www.Send();
    }

    public void SendHighscore()
    {
        if (_inputField.text.Length > 0)
        {
            StartCoroutine(Upload());

            System.Threading.Thread.Sleep(100);
            while (!_www.isDone && _www.error == null)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        QuitToMenu();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitToWindows()
    {
        Application.Quit();
    }
}
