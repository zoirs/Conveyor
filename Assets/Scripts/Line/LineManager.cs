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
            
            TextAsset level = Resources.Load<TextAsset>("Levels/Level");
            LevelDto fromJson = JsonUtility.FromJson<LevelDto>(level.text);
            
            _manufacturers = new List<Company>();
            foreach (CompanyDto company in fromJson.companies) {
                if (company.manufactureType == "manufacture") {
                    ManufacturerController manufacturer = _factoryManufacture.Create();
                    manufacturer.Init(company.productType, company.position);
                    _manufacturers.Add(manufacturer);                    
                }
                if (company.manufactureType == "city") {
                    CityController cityController = _cityManager.Create(company.position);
                    _manufacturers.Add(cityController);  
                }
            }
            foreach (LineDto line in fromJson.lines) {
                Debug.Log("create line");
                _factoryLine.Create(new List<Vector3>(), new List<Company>() {_manufacturers[line.indexFirst], _manufacturers[line.indexSecond]});
            }
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