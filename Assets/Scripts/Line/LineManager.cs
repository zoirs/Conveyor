using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Line {
    public class LineManager {
        private GameSettingsInstaller.LineMaterialsSettings _lineMaterialsSettings;
        private DrowLineComponent.Factory _factoryLine;
        private ManufacturerController.Factory _factoryManufacture;
        private CityManager _cityManager;

        readonly Dictionary<LineType, LineController> _lines = new Dictionary<LineType, LineController>();
        private List<Company> _manufacturers;

        public LineManager(GameSettingsInstaller.LineMaterialsSettings lineMaterialsSettings,
            DrowLineComponent.Factory factoryLine,
            ManufacturerController.Factory factoryManufacture,
            CityManager factoryStations) {
            _factoryManufacture = factoryManufacture;
            _lineMaterialsSettings = lineMaterialsSettings;
            _factoryLine = factoryLine;
            _cityManager = factoryStations;
        }

        public void Start() {
            List<Tuple<Vector3, bool>> positionsManufacture = new List<Tuple<Vector3, bool>>{
                new Tuple<Vector3, bool>(new Vector3(2f+3.5f, 9.5f),false),        //0
                new Tuple<Vector3, bool>(new Vector3(2f+6.5f, 11.5f),false),       //1
                new Tuple<Vector3, bool>(new Vector3(2f+6.5f, 8.5f),false),        //2
                new Tuple<Vector3, bool>(new Vector3(2f+9.5f, 13.5f),false),       //3
                new Tuple<Vector3, bool>(new Vector3(2f+9.5f, 7.5f),true),//      //
                new Tuple<Vector3, bool>(new Vector3(2f+11.5f, 11.5f),false),      //4
                new Tuple<Vector3, bool>(new Vector3(2f+11.5f, 9.5f),false),       //5
                new Tuple<Vector3, bool>(new Vector3(2f+14.5f, 11.5f),true),//    //
                new Tuple<Vector3, bool>(new Vector3(2f+14.5f, 9.5f),false),       //6
                new Tuple<Vector3, bool>(new Vector3(2f+14.5f, 7.5f),false),       //7

            };
            _manufacturers = new List<Company>();
            foreach (Tuple<Vector3, bool> tuple in positionsManufacture) {
                if (tuple.Item2) {
                    ManufacturerController manufacturer = _factoryManufacture.Create();
                    manufacturer.Init(tuple.Item1.x < 14f ? ProductType.Brown : ProductType.Yellow, tuple.Item1);
                    _manufacturers.Add(manufacturer);                    
                }
                else {
                    CityController cityController = _cityManager.Create(tuple.Item1);
//                    int stationCount = manufacturers.Count(q => q is CityController);
//                    cityController.Init((stationCount == 0) ? ProductType.Brown : ProductType.Yellow);
                    _manufacturers.Add(cityController);  
                }
            }

//            ManufacturerController first = _factoryManufacture.Create();
//            first.transform.position = new Vector3(8,8);
//            ManufacturerController second = _factoryManufacture.Create();
//            second.transform.position = new Vector3(14, 13);
//            DrowLineComponent drowLineComponent = _factoryLine.Create(new List<Vector3>() {
//                new Vector3(8, 8),
//                new Vector3(8, 11),
//                new Vector3(14, 13)
//            }, new List<ManufacturerController>() {manufacturers[0], manufacturers[1]});

            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[0], _manufacturers[1]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[0], _manufacturers[2]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[1], _manufacturers[2]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[1], _manufacturers[3]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[2], _manufacturers[4]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[3], _manufacturers[5]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[4], _manufacturers[6]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[5], _manufacturers[7]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[6], _manufacturers[8]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[7], _manufacturers[8]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[8], _manufacturers[9]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[5], _manufacturers[6]});
//            _lines.Add(LineType.NOT_USED, drowLineComponent.gameObject.GetComponent<LineController>());
        }

        public void Create(LineType lineType) {
            DrowLineComponent drowLineComponent;
            if (!_lines.ContainsKey(lineType)) {
                drowLineComponent = null;//_factoryLine.Create();
                _lines.Add(lineType, drowLineComponent.gameObject.GetComponent<LineController>());
            }
            else {
                drowLineComponent = _lines[lineType].GetComponent<DrowLineComponent>();
            }

            switch (lineType) {
                case LineType.RED:
                    drowLineComponent.Init(_lineMaterialsSettings.Red);
                    break;
                case LineType.BLUE:
                    drowLineComponent.Init(_lineMaterialsSettings.Blue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineType), lineType, null);
            }
        }

        public List<Company> Manufacturers => _manufacturers;
    }
}