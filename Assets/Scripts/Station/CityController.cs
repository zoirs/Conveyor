using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Line;
using ModestTree;
using UnityEngine;
using Zenject;

public class CityController : MonoBehaviour, Company {
    [SerializeField]
    private BoxCollider area;
    [SerializeField]
    private TextMesh needCountText;
    
    private SignalBus _signalBus;
    private Task curentTask;

    private HashSet<ProductEntity> products = new HashSet<ProductEntity>();
    private int _areaValue = int.MinValue;
    private List<LineController> _lines = new List<LineController>();
    private GameSettingsInstaller.ProductSettings _materialsProductSettings;
    private GuiHandler _guiHandler;
    private MoneyService _moneyService;

    [Inject]
    public void Construct(SignalBus signalBus, GameSettingsInstaller.ProductSettings materialsProductSettings, MoneyService moneyService) {
        _moneyService = moneyService;
        _signalBus = signalBus;
        _materialsProductSettings = materialsProductSettings;
        _guiHandler = GameObject.Find("Gui").GetComponent<GuiHandler>();
    }

    private void Start() {
    }

    public ProductType GetProductType() {
        return curentTask != null ? curentTask.ProductType : ProductType.Empty;
    }

    public void Enter(ProductEntity productEntity) {
        // вызывать этот метод явно, или по событию EnterToStationSignal ?
        products.Add(productEntity);
        UpdateNeedCountText();
        _signalBus.Fire<AddProductSignal>();
    }
    
    public bool InStationArea(Vector2 point) {
        return area.bounds.Contains(point);
    }

    public void AddLine(LineController lineController) {
        _lines.Add(lineController);
    }

    public LineController GetNextLine(LineController currentLine) {
        foreach (LineController lineController in _lines) {
            if (lineController.LineType == LineType.NOT_USED || lineController == currentLine) {
                continue;
            }

            if (lineController.LineType == currentLine.LineType) {
                return lineController;
            }
        }
        return null;
    }

    public void AddProduct(ProductEntity product) {
        products.Add(product);
        _signalBus.Fire<AddProductSignal>();
        if (curentTask.ProductType == product.ProductType) {
            curentTask.AddProgress();
            _moneyService.Plus(curentTask.Coast);
        }

        if (curentTask.IsComplete()) {
            CompleteTask(curentTask);
        }
        UpdateNeedCountText();
    }

    private void CompleteTask(Task task) {
        Debug.Log("Task complete");
        task.Customer = null;
        curentTask = null;
        UpdateNeedCountText();
        _guiHandler.RemoveCityTaskPanel(this);
        _signalBus.Fire<TaskCompleteSignal>();
    }

    public void AddTask(Task task) {
        Debug.Log("Add Task");
        curentTask = task;
        task.Customer = this;
        UpdateNeedCountText();
        _guiHandler.AddCityTaskPanel(this);
    }

    public void UpdateNeedCountText() {
        needCountText.gameObject.SetActive(curentTask != null && !curentTask.IsComplete());
        if (needCountText.gameObject.activeSelf && curentTask != null) {
            Debug.Log("curentTask " + curentTask.NeedCount + " "+ curentTask.Progress);
            GetComponent<Renderer>().material = curentTask.ProductType.GetMaterial(_materialsProductSettings);
            int need = curentTask.NeedCount - curentTask.Progress;
            needCountText.text = need.ToString();
        }
        else {
            GetComponent<Renderer>().material = ProductType.Empty.GetMaterial(_materialsProductSettings);
        }
    }

    public Vector2 getPanelPosition() {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    public int AreaValue => _areaValue;

    public Task CurentTask => curentTask;

    public class Factory : PlaceholderFactory<CityController> { }
}