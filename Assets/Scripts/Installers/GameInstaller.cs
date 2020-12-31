using System.Collections.Generic;
using DefaultNamespace;
using Line;
using Main;
using Map;
using UnityEngine;
using Zenject;

// документация https://github.com/modesttree/Zenject
public class GameInstaller : MonoInstaller {
    [Inject] GameSettingsInstaller.PrefabSettings _prefabs;

    public override void InstallBindings() {
        Container.Bind<ScreenService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PeopleManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<StationManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LineManager>().AsSingle();

        Container.BindFactory<TrainController, TrainController.Factory>()
            .FromComponentInNewPrefab(_prefabs.TrainPrefab)
            .WithGameObjectName("Train")
            .UnderTransformGroup("Trains");

//        Container.BindFactory<DrowLineComponent, DrowLineComponent.Factory>()
//            .FromComponentInNewPrefab(_prefabs.LinePrefab)
//            .WithGameObjectName("Line")
//            .UnderTransformGroup("Lines");

        Container.BindFactory<List<Vector3>, List<Company>, DrowLineComponent, DrowLineComponent.Factory>()
            .FromMethod(CreateLine);

        Container.BindFactory<StationController, StationController.Factory>()
            .FromComponentInNewPrefab(_prefabs.StationPrefab)
            .WithGameObjectName("Station")
            .UnderTransformGroup("Stations");

        Container.BindFactory<ManufacturerController, ManufacturerController.Factory>()
            .FromComponentInNewPrefab(_prefabs.ManufacturePrefab)
            .WithGameObjectName("Manufacture")
            .UnderTransformGroup("Manufactures");

//        Container.BindFactory<ProductEntity, ProductEntity.Factory>()
//            .FromComponentInNewPrefab(_prefabs.PeoplePrefab)
//            .WithGameObjectName("People")
//            .UnderTransformGroup("Peoples");

        InstallSignals();
    }

    private DrowLineComponent CreateLine(DiContainer subContainer, List<Vector3> points, List<Company> manufactures) {
        DrowLineComponent component = subContainer.InstantiatePrefabForComponent<DrowLineComponent>(_prefabs.LinePrefab,  new object[] { points, manufactures });
        component.gameObject.transform.parent = GameObject.Find("Lines").transform;
        return component;
    }

    void InstallSignals() {
        // Every scene that uses signals needs to install the built-in installer SignalBusInstaller
        // Or alternatively it can be installed at the project context level (see docs for details)
        SignalBusInstaller.Install(Container);

        // Signals can be useful for game-wide events that could have many interested parties
        Container.DeclareSignal<EnterToStationSignal>();
        Container.DeclareSignal<ChangeLevelSignal>();
    }
}