using System;
using DefaultNamespace;
using Line;
using Map;
using UnityEngine;
using Zenject;

public class GuiHandler : MonoBehaviour , IDisposable, IInitializable  {
    [SerializeField] GUIStyle _timeStyle;

    [Inject] private TrainController.Factory _factoryTrain;
    [Inject] private StationManager _stationManager;
    [Inject] private LineManager _lineManager;
    [Inject] private MoneyService _moneyService;
    [Inject] private MapService _mapService;
    [Inject] private PeopleManager _peopleManager;
    [Inject] private MoneyService.PriceSettings PriceSettings;
    [Inject] private SignalBus _signalBus;
    [Inject] private GameSettingsInstaller.ProductSettings _product;

    Rect windowRect = new Rect(300, 200, 180, 200);
    private bool show;
    private LineController _selectedLine;
    private ManufacturerController _selectedManufacture;

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            PlayingGui();
        }
        GUILayout.EndArea();
    }

    void PlayingGui() {
        if (_selectedLine != null) {
            GUILayout.Window(0,  new Rect(_selectedLine.getPanelPosition().x, Screen.height - _selectedLine.getPanelPosition().y, 180, 50), LineWindow, "Линия");            
        }
        if (_selectedManufacture != null) {
            GUILayout.Window(0,  new Rect(_selectedManufacture.getPanelPosition().x, Screen.height - _selectedManufacture.getPanelPosition().y, 180, 70), ManufactureWindow, "");            
        }

        GUILayout.BeginVertical();
        {
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(30);

                if (show) {
                    GUILayout.Window(0, windowRect, DoMyWindow, "Открыт новый район");
                }

                GUILayout.BeginVertical();
                GUILayout.Label("Balance: " + _moneyService.balance);
                GUILayout.Label("District: " + (_mapService.currentLevel + 1));
                GUILayout.Label("People: " + _peopleManager.PeoplesCount);
                GUILayout.EndVertical();
                
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();

                if (GUILayout.Button("Create")) {
                    _moneyService.Minus(PriceSettings.lineElement);
                    _lineManager.Create();                    
                }
                
//                if (GUILayout.Button("Create Red Line " + PriceSettings.lineElement + "$")) {
//                    _moneyService.Minus(PriceSettings.lineElement);
//                    _lineManager.Create(LineType.RED);                    
//                }

//                if (GUILayout.Button("Create Blue Line " + PriceSettings.lineElement + "$")) {
//                    _moneyService.Minus(PriceSettings.lineElement);
//                    _lineManager.Create(LineType.BLUE);
//                }

                if (GUILayout.Button("Create Train " + PriceSettings.lineElement + "$")) {
                    _moneyService.Minus(PriceSettings.lineElement);
                    _factoryTrain.Create();
                }

//                if (GUILayout.Button("Create Station " + PriceSettings.station + "$")) {
//                    _moneyService.Minus(PriceSettings.station);
//                    _stationManager.Create();
//                }
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void ManufactureWindow(int id) {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label(_product.Yellow,GUILayout.MaxWidth(10));
        GUILayout.Label(_selectedManufacture.YellowCownt());
        if (GUILayout.Button("Add")) {
            _selectedManufacture.Produce(ProductType.Yellow);
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(_product.Brown,GUILayout.MaxWidth(10));
        GUILayout.Label(_selectedManufacture.BrownCownt());
        if (GUILayout.Button("Add")) {
            _selectedManufacture.Produce(ProductType.Brown);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();        
    }


    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        // This button will size to fit the window

    }

    void LineWindow(int windowID)
    {
        if (GUILayout.Button("Red")) {
            _selectedLine.LineType = LineType.RED;
            _selectedLine = null;
        }
        if (GUILayout.Button("Blue")) {
            _selectedLine.LineType = LineType.BLUE;
            _selectedLine = null;
        }
        if (GUILayout.Button("Not used")) {
            _selectedLine.LineType = LineType.NOT_USED;
            _selectedLine = null;
        }
    }

    public void Dispose() {
        _signalBus.Unsubscribe<ChangeLevelSignal>(OnDistrictOpen);
    }

    public void Initialize() {
        _signalBus.Subscribe<ChangeLevelSignal>(OnDistrictOpen);
    }

    private void OnDistrictOpen() {
        show = true;
    }

    public void ShowPanelFor(LineController selectedLine) {
        _selectedManufacture = null;
        if (_selectedLine == selectedLine) {
            _selectedLine = null;
            return;
        }
        _selectedLine = selectedLine;
    }
    
    public void ShowPanelFor(ManufacturerController selectedManufacture) {
        _selectedLine = null;
        if (_selectedManufacture == selectedManufacture) {
            _selectedManufacture = null;
            return;
        }

        _selectedManufacture = selectedManufacture;
    }

    public void HidePanel() {
        _selectedManufacture = null;
        _selectedLine = null;        
    }
}