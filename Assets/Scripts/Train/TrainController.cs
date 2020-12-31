using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Line;
using Train;
using UnityEngine;
using Zenject;

public class TrainController : MonoBehaviour {
    [SerializeField] private TextMesh countText;

    [Inject] private Settings _settings;

    private bool forward = true;
    private Vector3 _dir;
    private int index;
    private TrainState _state;
    private LineController _currentLine;
    private float speedKoef = 1;

    private List<ProductEntity> peoples = new List<ProductEntity>();


    private void Update() {
        switch (_state) {
            case TrainState.WaitingSetLine:
                Vector2 tmpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(tmpPosition, transform.position) > 0.5f) {
                    transform.position = tmpPosition;
                }

                if (Input.GetMouseButtonDown(0)) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {
                        Debug.Log(hit.transform.name);
                        LineController line = hit.transform.GetComponentInParent<LineController>();
                        if (line != null) {
                            _currentLine = line;
                            line.AddTrain(this);
                            index = 0;
                            _state = TrainState.Going;
                        }
                        else {
                            Destroy(gameObject);
                        }
                    }
                }

                break;
            case TrainState.Going:
                float dist = Vector3.Distance(_currentLine.GetPointPosition(index), transform.position);
                if (dist <= 0.2f) {
                    speedKoef = _currentLine.IsStation(index) ? dist * 2 : 0.7f;
                }
                else {
                    speedKoef = 1;
                }

                float speed = speedKoef * _settings.speed * Time.deltaTime;
                transform.position =
                    Vector3.MoveTowards(transform.position, _currentLine.GetPointPosition(index), speed);
//                transform.Translate(_dir.normalized * speedKoef * _settings.speed * Time.deltaTime);

                if (dist <= 0.1f) {
//                    if (_currentLine.IsStation(index)) {
                    _state = TrainState.OnStation;
//                    } else {
//                        _state = TrainState.OnTurn;                                                
//                    }
                }

                break;
            case TrainState.OnTurn:
                if (forward) {
                    index++;
                }
                else {
                    index--;
                }

                _dir = _currentLine.GetPointPosition(index) - transform.position;
                _state = TrainState.Going;
                break;
            case TrainState.OnStation:
                Company company = _currentLine.GetCompany(index);
                if (company is ManufacturerController) {
                    CollectProduct((ManufacturerController) company);
                }

                if (company is StationController) {
                    DropProduct((StationController) company);
                }

//                manufacturerController;
//                DropPeople(stationController);

                LineController nextLine = company.GetNextLine(_currentLine);
                if (nextLine != null) {
                    _currentLine = nextLine;
                }

                index = _currentLine.GetIndex(company);

                if (_currentLine.IsLast(index) && forward) {
                    forward = false;
                }

                if (_currentLine.IsFirst(index) && !forward) {
                    forward = true;
                }

                if (forward) {
                    index++;
                }
                else {
                    index--;
                }

                _dir = _currentLine.GetPointPosition(index) - transform.position;
                _state = TrainState.Going;

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CollectProduct(ManufacturerController manufacture) {
        ProductEntity canSellProduct = ExistConsumerOnLineFor(manufacture, _currentLine, manufacture.GetProducts());
        if (canSellProduct == null) {
            return;
        }

        bool productEntity = manufacture.TakeFromManufacture(canSellProduct);
        if (productEntity) {
            peoples.Add(canSellProduct);
            countText.text = peoples.Count.ToString();
        }
    }

    private void DropProduct(StationController stationController) {
        Debug.Log("drop on " + stationController.GetProductType());
        for (int i = peoples.Count - 1; i >= 0; i--) {
            ProductEntity product = peoples[i];
            Debug.Log("product " + product.ProductType);
            if (product.ProductType == stationController.GetProductType()) {
                peoples.RemoveAt(i);
                countText.text = peoples.Count.ToString();
                stationController.AddProduct(product);
            }
        }
    }

    public ProductEntity ExistConsumerOnLineFor(Company manufacturer, LineController currentLine, List<ProductEntity> products) {
        if (products.Count == 0) {
            return null;
        }

        Debug.Log("Find product " + products.Count + " " + products[0]._productType + " on " + manufacturer);
        LineController lineController = manufacturer.GetNextLine(currentLine);
        if (lineController == null) {
            return null;
        }

        Company companySecond = lineController.GetAnotherCompany(manufacturer);
        Debug.Log("Find station "+ (companySecond));
//        Debug.Log("Find station StationController"+ (companySecond is StationController));
//        Debug.Log("Find station ManufacturerController"+ (companySecond is ManufacturerController));

        if (companySecond is StationController) {
            Debug.Log("Find station "+ ((StationController) companySecond).GetProductType());

            ProductEntity canSellProduct = products.Find(entity => entity._productType == ((StationController) companySecond).GetProductType());
            if (canSellProduct != null) {
                return canSellProduct;
            }
        }

//        LineController nextLine = companySecond.GetNextLine(lineController);
//        Debug.Log("next line "+ (nextLine));
//        if (nextLine == null) {
//            return false;
//        }

        return ExistConsumerOnLineFor(companySecond, lineController, products);
    }


    [Serializable]
    public class Settings {
        public float speed;
    }

    public class Factory : PlaceholderFactory<TrainController> { }
}