using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CityManager {
    [Inject] private CityController.Factory _factoryStation;
    [Inject] private MapService _mapService;
    
    private readonly List<CityController> _all = new List<CityController>();

    public CityController Create(Vector3 position) {
        CityController cityController = _factoryStation.Create();
        cityController.transform.position = position;
        _all.Add(cityController);
        return cityController;
    }

    public CityController FindNearest(Vector2 point) {
        return _all.Find(controller => {
            bool contains = controller.InStationArea(point);
            return contains;
        });
    }
    
    public CityController GetDistrictStation(Vector2 point) {
        int areaValue = _mapService.GetAreaValue(point);
        return _all.Find(controller => {
            bool contains = controller.AreaValue == areaValue;
            return contains;
        });
    }

    public void AddTask(Task task) {
        List<CityController> cityControllers = _all.FindAll(p => p.CurentTask == null);
        Debug.Log("_all.Count: " +_all.Count);
        Debug.Log("cityControllers.Count: " +cityControllers.Count);
        if (cityControllers.Count > 0) {
            int range = Random.Range(0, cityControllers.Count);
            Debug.Log("range  " +range);
            cityControllers[range].AddTask(task);            
        }
    }
}