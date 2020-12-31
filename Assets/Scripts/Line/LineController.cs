using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Line {
    public class LineController : MonoBehaviour{
        private List<StationController> _stations = new List<StationController>();
        private List<TrainController> _trains = new List<TrainController>();
        private LineRenderer _lineRenderer;
        private Company _first;
        private Company _second;
        private LineType _lineType;
        
        [Inject]
        private GameSettingsInstaller.LineMaterialsSettings _lineMaterialsSettings;

        private GuiHandler _guiHandler;

        private void Start() {
            _guiHandler = GameObject.Find("Gui").GetComponent<GuiHandler>();
            _lineRenderer = GetComponent<LineRenderer>();
            LineType = LineType.NOT_USED;
        }

        public void AddStation(StationController station) {
            _stations.Add(station);
        }
        
        public void AddTrain(TrainController train) {
            _trains.Add(train);
            _guiHandler.HidePanel();
        }

        public bool IsLast(int index) {
            return _lineRenderer.positionCount -1 <= index;
        }
        
        public bool IsFirst(int index) {
            return index == 0;
        }

        public Vector3 GetPointPosition(int index) {
            return _lineRenderer.GetPosition(index);
        }
        
        public Vector2 getPanelPosition() {
            float x = (_first.gameObject.transform.position.x + _second.gameObject.transform.position.x)/2;
            float y = (_first.gameObject.transform.position.y + _second.gameObject.transform.position.y) / 2;
            return Camera.main.WorldToScreenPoint(new Vector3(x, y));
        }

        public bool IsStation(int index) {
            if (_stations.IsEmpty()) {
                return false;
            }
            Vector3 stationPosition = GetPointPosition(index);
            Debug.Log("Index " + index);
            return _stations.Exists(controller => {
                Debug.Log("Index " + index + ", index pos " + stationPosition + ", controller.transform.position " + controller.transform.position);
                return Vector2.Distance(controller.transform.position, stationPosition) < 0.1f;
            });
        }

        public StationController GetStation(int index) {
            Vector3 stationPosition = GetPointPosition(index);
            Debug.Log("Index " + index);
            return _stations.First(controller => {
                Debug.Log("Index " + index + ", index pos " + stationPosition + ", controller.transform.position " + controller.transform.position);
                return Vector2.Distance(controller.transform.position, stationPosition) < 0.1f;
            });
        }
        
        public Company GetCompany(int index) {
            if (index == 0) {
                return _first;
            }
            if (index == 1) {
                return _second;
            }

            return null;
        }
        
        public Company GetAnotherCompany(Company company) {
            if (_first == company && _second != company) {
                return _second;
            }
            if (_first != company && _second == company) {
                return _first;
            }
            
            throw new Exception();
        }

        public void AddCompany(Company first, Company second) {
            _first = first;
            _second = second;
        }

        public LineType LineType {
            get { return _lineType; }
            set {
                _lineRenderer.material = value.GetMaterial(_lineMaterialsSettings);
                _lineType = value;
            }
        }

        public int GetIndex(Company manufacturerController) {
            if (_first == manufacturerController) {
                return 0;
            }
            if (_second == manufacturerController) {
                return 1;
            }

            return -100;
        }
    }
}