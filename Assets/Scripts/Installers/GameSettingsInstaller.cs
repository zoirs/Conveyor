using System;
using DefaultNamespace;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public PrefabSettings Prefabs;
    public LineMaterialsSettings LineMaterials;
    public PeopleMaterialsSettings PeopleMaterials;
    public PeopleSettings People;
    public ProductSettings Product;
    public TrainController.Settings TrainSetting;
    public MoneyService.PriceSettings PriceSettings;

    [Serializable]
    public class PeopleSettings
    {
        public PeopleManager.Settings Spawner;
    }
    
    [Serializable]
    public class PrefabSettings
    {
        public GameObject LinePrefab;
        public GameObject TrainPrefab;
        public GameObject StationPrefab;
        public GameObject ManufacturePrefab;
        public GameObject PeoplePrefab;
    }
    
    [Serializable]
    public class LineMaterialsSettings
    {
        public Material NotUsed;
        public Material Red;
        public Material Blue;
    }  
    
    [Serializable]
    public class ProductSettings
    {
        public Material YellowMaterial;
        public Material BrownMaterial;
        public Texture Yellow;
        public Texture Brown;
    }
    
    [Serializable]
    public class PeopleMaterialsSettings
    {
        public Material Simple;
        public Material Wait;
        public Material WaitAgry;
        public Material _area0;
        public Material _area1;
        public Material _area2;
        public Material _area3;
    }
    
    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(LineMaterials);
        Container.BindInstance(PeopleMaterials);
        Container.BindInstance(People.Spawner);
        Container.BindInstance(TrainSetting);
        Container.BindInstance(PriceSettings);
        Container.BindInstance(Product);
    }
}
