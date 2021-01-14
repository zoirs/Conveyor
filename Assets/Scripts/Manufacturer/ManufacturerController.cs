using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Line;
using ModestTree;
using UnityEngine;
using Zenject;

public class ManufacturerController : MonoBehaviour, Company {
    private GuiHandler _guiHandler;
    private List<LineController> _lineControllers = new List<LineController>();
    private List<ProductEntity> products = new List<ProductEntity>();

    [SerializeField] private TextMesh countText;

    private GameSettingsInstaller.ProductSettings _materialsProductSettings;
    private GameSettingsInstaller.ManufactureSettings _manufactureSettings;

    private ProductType _productType;

    [Inject]
    public void Construct(GameSettingsInstaller.ProductSettings materialsProductSettings, GameSettingsInstaller.ManufactureSettings manufactureSettings) {
        _materialsProductSettings = materialsProductSettings;
        _manufactureSettings = manufactureSettings;
        _guiHandler = GameObject.Find("Gui").GetComponent<GuiHandler>();
    }

    private void Start() {
        StartCoroutine(ProduceProduct());
        _guiHandler.AddManufacturePanel(this);
    }

    public void Init(ProductType productType, Vector3 position) {
        transform.position = position;
        _productType = productType;
        GetComponent<Renderer>().material = _productType.GetMaterial(_materialsProductSettings);
    }

    IEnumerator ProduceProduct() {
        if (products.Count < 7) {
            Produce();
        }

        yield return new WaitForSeconds(_manufactureSettings.produceSpeedSeconds);
        StartCoroutine(ProduceProduct());
    }

    private void OnGUI() {
        // if (Event.current.type == EventType.MouseDown) {
        // if (ClickOnManufacture()) {
        // _guiHandler.ShowPanelFor(this);                
        // }       
        // }

//        if (Input.GetMouseButtonDown(0)) {
//            if (ClickOnManufacture()) {
//                _guiHandler.ShowPanelFor(this);                
//            }
//        }
    }

    public void Produce() {
        ProductEntity productEntity = new ProductEntity(_productType);
        products.Add(productEntity);
        countText.text = products.Count.ToString();
    }

    private bool ClickOnManufacture() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        foreach (RaycastHit raycastHit in raycastHits) {
//            Debug.Log(raycastHit.transform.gameObject);
            if (raycastHit.transform.gameObject == gameObject) {
                return true;
            }
        }

        return false;
    }

    public Vector2 getPanelPosition() {
        return Camera.main.WorldToScreenPoint(transform.position);
//        return _lineRenderer.GetPosition(2);
    }

    public class Factory : PlaceholderFactory<ManufacturerController> { }

    public void AddLine(LineController lineController) {
        _lineControllers.Add(lineController);
    }

    public ProductEntity GoOnePeopleToTrain() {
        if (products.IsEmpty()) {
            return null;
        }

        ProductEntity people = products.First();
        products.Remove(people);
        countText.text = products.Count.ToString();
        return people;
    }

    public bool TakeFromManufacture(ProductEntity productEntity) {
        if (products.IsEmpty()) {
            return false;
        }

        bool result = products.Remove(productEntity);
        countText.text = products.Count.ToString();
        return result;
    }

    public LineController GetNextLine(LineController currentLine) {
        foreach (LineController lineController in _lineControllers) {
            if (lineController.LineType == LineType.NOT_USED || lineController == currentLine) {
                continue;
            }

            if (lineController.LineType == currentLine.LineType) {
                return lineController;
            }
        }

        return null;
    }

    public int ProductsCount() {
        return products.Count();
    }

    public List<ProductEntity> GetProducts() {
        return products;
    }

    public ProductType ProductType => _productType;
}