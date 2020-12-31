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
        private StationController.Factory _factoryStations;

        readonly Dictionary<LineType, LineController> _lines = new Dictionary<LineType, LineController>();

        public LineManager(GameSettingsInstaller.LineMaterialsSettings lineMaterialsSettings,
            DrowLineComponent.Factory factoryLine,
            ManufacturerController.Factory factoryManufacture,
            StationController.Factory factoryStations) {
            _factoryManufacture = factoryManufacture;
            _lineMaterialsSettings = lineMaterialsSettings;
            _factoryLine = factoryLine;
            _factoryStations = factoryStations;
        }

        public void Create() {
            List<Tuple<Vector3, bool>> positionsManufacture = new List<Tuple<Vector3, bool>>{
                new Tuple<Vector3, bool>(new Vector3(3.5f, 9.5f),true),        //0
                new Tuple<Vector3, bool>(new Vector3(6.5f, 11.5f),true),       //1
                new Tuple<Vector3, bool>(new Vector3(6.5f, 8.5f),true),        //2
                new Tuple<Vector3, bool>(new Vector3(9.5f, 13.5f),true),       //3
                new Tuple<Vector3, bool>(new Vector3(9.5f, 7.5f),false),//      //
                new Tuple<Vector3, bool>(new Vector3(11.5f, 11.5f),true),      //4
                new Tuple<Vector3, bool>(new Vector3(11.5f, 9.5f),true),       //5
                new Tuple<Vector3, bool>(new Vector3(14.5f, 11.5f),false),//    //
                new Tuple<Vector3, bool>(new Vector3(14.5f, 9.5f),true),       //6
                new Tuple<Vector3, bool>(new Vector3(14.5f, 7.5f),true),       //7

            };
            List<Company> manufacturers = new List<Company>();
            foreach (Tuple<Vector3, bool> tuple in positionsManufacture) {
                if (tuple.Item2) {
                    ManufacturerController manufacturer = _factoryManufacture.Create();
                    manufacturer.transform.position = tuple.Item1;
                    manufacturers.Add(manufacturer);                    
                }
                else {
                    StationController stationController = _factoryStations.Create();
                    stationController.transform.position = tuple.Item1;
                    int stationCount = manufacturers.Count(q => q is StationController);
                    stationController.Init((stationCount == 0) ? ProductType.Brown : ProductType.Yellow);
                    manufacturers.Add(stationController);  
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

            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[0], manufacturers[1]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[0], manufacturers[2]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[1], manufacturers[2]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[1], manufacturers[3]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[2], manufacturers[4]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[3], manufacturers[5]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[4], manufacturers[6]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[5], manufacturers[7]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[6], manufacturers[8]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[7], manufacturers[8]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[8], manufacturers[9]});
            _factoryLine.Create(new List<Vector3>(), new List<Company>() {manufacturers[5], manufacturers[6]});
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
    }
}