using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Line;
using ModestTree;
using UnityEngine;
using Zenject;

public class StationController : MonoBehaviour, Company {
    [SerializeField]
    private BoxCollider area;
    [SerializeField]
    private TextMesh countText;
    
    private SignalBus _signalBus;
    private ProductType _productType;

    private bool _completed;
    private HashSet<ProductEntity> peoples = new HashSet<ProductEntity>();
    private MapService _mapService;
    private int _areaValue = int.MinValue;
    private List<LineController> _lines = new List<LineController>();
    private GameSettingsInstaller.ProductSettings _materialsProductSettings;

    [Inject]
    public void Construct(MapService mapService, SignalBus signalBus, GameSettingsInstaller.ProductSettings materialsProductSettings) {
        _signalBus = signalBus;
        _mapService = mapService;
        _materialsProductSettings = materialsProductSettings;
    }

    
    public void Init(ProductType productType) {
        _productType = productType;
         GetComponent<MeshRenderer>().material = _productType.GetMaterial(_materialsProductSettings);
    }
    
    public ProductType GetProductType() {
        return _productType;
    }

    private void Update() {
//        if (!_completed) {
//            Vector2 tmpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//
//            if (Vector2.Distance(tmpPosition, transform.position) > 0.1f) {
//                Vector2 sector = _mapService.GetXY(tmpPosition);
//                transform.position = new Vector2(sector.x + 0.5f, sector.y + 0.5f);;
//            }
//
//            if (Input.GetMouseButtonDown(0)) {
//                _areaValue = _mapService.GetAreaValue(transform.position);
//                _completed = true;
//            }
//        }
    }

    public void Enter(ProductEntity productEntity) {
        // вызывать этот метод явно, или по событию EnterToStationSignal ?
        peoples.Add(productEntity);
        countText.text = peoples.Count.ToString();
        _signalBus.Fire<EnterToStationSignal>();
    }
    
    public ProductEntity GoOnePeopleToTrain() {
        if (peoples.IsEmpty()) {
            return null;
        }

        ProductEntity product = peoples.First();
        peoples.Remove(product);
        countText.text = peoples.Count.ToString();
        return product;
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
        peoples.Add(product);
        countText.text = peoples.Count.ToString();
        _signalBus.Fire<EnterToStationSignal>();
    }

    public int AreaValue => _areaValue;

    public class Factory : PlaceholderFactory<StationController> { }
}