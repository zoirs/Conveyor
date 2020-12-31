using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StationManager {
    [Inject] private StationController.Factory _factoryStation;
    [Inject] private MapService _mapService;
    
    private readonly List<StationController> _all = new List<StationController>();

    public void Create() {
        StationController stationController = _factoryStation.Create();
        _all.Add(stationController);
    }

    public StationController FindNearest(Vector2 point) {
        return _all.Find(controller => {
            bool contains = controller.InStationArea(point);
            return contains;
        });
    }
    
    public StationController GetDistrictStation(Vector2 point) {
        int areaValue = _mapService.GetAreaValue(point);
        return _all.Find(controller => {
            bool contains = controller.AreaValue == areaValue;
            return contains;
        });
    }
}