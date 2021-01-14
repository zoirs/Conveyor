using System;
using System.Collections.Generic;
using DefaultNamespace;
using Line;
using Main;
using Map;
using Train;
using UnityEngine;
using Zenject;

public class GuiHandler : MonoBehaviour, IDisposable, IInitializable {
    [SerializeField] GUIStyle _timeStyle;

    [Inject] private TrainManager _trainManager;
    // [Inject] private CityManager _cityManager;
    // [Inject] private LineManager _lineManager;
    [Inject] private MoneyService _moneyService;
    // [Inject] private MapService _mapService;
    // [Inject] private PeopleManager _peopleManager;
    [Inject] private MoneyService.PriceSettings PriceSettings;
    [Inject] private GameController _gameController;

    [Inject] private SignalBus _signalBus;
    [Inject] private GameSettingsInstaller.ProductSettings _product;

    Rect windowRect = new Rect(300, 200, 180, 200);
    private bool show;
    private LineController _selectedLine;
    private List<ManufacturerController> manufactures = new List<ManufacturerController>();
    private List<CityController> tasks = new List<CityController>();

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            PlayingGui();
        }
        GUILayout.EndArea();
    }

    void PlayingGui() {
        if (_selectedLine != null) {
            GUILayout.Window(100,
                new Rect(_selectedLine.getPanelPosition().x, Screen.height - _selectedLine.getPanelPosition().y, 180,
                    50), LineWindow, "Линия");
        }

        for (int i = 0; i < manufactures.Count; i++) {
            ManufacturerController manufacture = manufactures[i];
            GUILayout.Window(i,
                new Rect(manufacture.getPanelPosition().x, Screen.height - manufacture.getPanelPosition().y - 40,
                    15 * manufacture.ProductsCount(), 20),
                (id) => ManufactureWindow(id, manufacture), "");
        }

        for (int i = 0; i < tasks.Count; i++) {
            CityController cityController = tasks[i];
            GUILayout.Window(50 + i,
                new Rect(cityController.getPanelPosition().x, Screen.height - cityController.getPanelPosition().y - 40,
                    100, 20),
                (id) => TaskWindow(id, cityController), "Надо:");
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
                GUILayout.Label("Баланс: " + _moneyService.balance);

                if (_gameController.State == GameStates.Playing) {
                    if (GUILayout.Button("Купить поезд $" + PriceSettings.train)) {
                        _moneyService.Minus(PriceSettings.train);
                        _trainManager.Create();
                    }
                }

                for (var i = 0; i < _trainManager.Trains.Count; i++) {
                    TrainController train = _trainManager.Trains[i];
                    GUILayout.Label("Поезд " + (i + 1), GUILayout.Height(20));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Скорость: " + train.Speed(), GUILayout.Height(20));
                    if (GUILayout.Button("Увеличить $" + PriceSettings.trainSpeed)) {
                        _moneyService.Minus(PriceSettings.trainSpeed);
                        train.UpgradeSpeed();
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Вагонов: 1", GUILayout.Height(20));
                    GUI.enabled = false;
                    if (GUILayout.Button("Увеличить $" + PriceSettings.trainWagon)) {
                        _moneyService.Minus(PriceSettings.trainWagon);
                        //
                    }
                    GUI.enabled = true;

                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(train.StateText());
                    if (train.State == TrainState.Depo) {
                        if (GUILayout.Button("На линию")) {
                            train.GoToLine();
                        }
                    }
                    else {
                        if (GUILayout.Button("В депо")) {
                            train.GoToDepo();
                        }
                    }


                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();

                if (_gameController.State == GameStates.WaitingToStart) {
                    if (GUILayout.Button("Начать игру")) {
                        _gameController.StartGame();
                    }
                }

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void ManufactureWindow(int id, ManufacturerController manufacturerController) {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < manufacturerController.ProductsCount(); i++) {
            GUILayout.Label(manufacturerController.ProductType.GetTexture(_product), GUILayout.MaxWidth(10));
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void TaskWindow(int id, CityController cityController) {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        int count = cityController.CurentTask.NeedCount - cityController.CurentTask.Progress;
        GUILayout.Label(count.ToString());
        GUILayout.Label(cityController.CurentTask.ProductType.GetTexture(_product));
        GUILayout.Label(" = " + (cityController.CurentTask.Coast * count) + "$");

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }


    // Make the contents of the window
    void DoMyWindow(int windowID) {
        // This button will size to fit the window
    }

    void LineWindow(int windowID) {
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
        if (_selectedLine == selectedLine) {
            _selectedLine = null;
            return;
        }

        _selectedLine = selectedLine;
    }

    // public void ShowPanelFor(ManufacturerController selectedManufacture) {
    //     _selectedLine = null;
    //     if (manufactures == selectedManufacture) {
    //         manufactures = null;
    //         return;
    //     }
    //
    //     manufactures = selectedManufacture;
    // }

    public void HidePanel() {
        _selectedLine = null;
    }

    public void AddManufacturePanel(ManufacturerController manufacturerController) {
        manufactures.Add(manufacturerController);
        Debug.Log("AddManufacturePanel" + manufactures.Count);
    }

    public void AddCityTaskPanel(CityController cityController) {
        tasks.Add(cityController);
    }

    public void RemoveCityTaskPanel(CityController cityController) {
        tasks.Remove(cityController);
    }
}