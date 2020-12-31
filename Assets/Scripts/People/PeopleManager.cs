using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class PeopleManager : ITickable, IFixedTickable {
        readonly List<ProductEntity> _peoples = new List<ProductEntity>();

        readonly Settings _settings;
        readonly MapService _mapService;


        bool _started;

        public PeopleManager(Settings settings, MapService mapService) {
            _settings = settings;
            _mapService = mapService;
        }

        public void Start() {
//            Assert.That(!_started);
            _started = true;

            ResetAll();

            for (int i = 0; i < _settings.startingSpawns; i++) {
                SpawnNext();
            }
        }

        public void SpawnNext() {
            ProductEntity product = new ProductEntity(ProductType.Yellow);
            _peoples.Add(product);
        }

        Vector3 GetRandomDirection() {
            var theta = Random.Range(0, Mathf.PI * 2.0f);
            return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        }

        Vector3 GetRandomStartPosition() {
            Vector3 randomStartPosition = new Vector3(Random.Range(5, 15.0f), Random.Range(5.0f, 15.0f));
            int areaValue = _mapService.GetAreaValue(randomStartPosition);
            while (areaValue > _mapService.currentLevel) {
                randomStartPosition = new Vector3(Random.Range(5, 15.0f), Random.Range(5.0f, 15.0f));
                areaValue = _mapService.GetAreaValue(randomStartPosition);
            }
            return randomStartPosition;
        }

        void ResetAll() {
//            foreach (var asteroid in _peoples) {
//                GameObject.Destroy(asteroid.gameObject);
//            }

            _peoples.Clear();
        }


        public void Tick() {
//            for (int i = 0; i < _peoples.Count; i++) {
//                _peoples[i].Tick();
//            }
        }

        public void FixedTick() {
//            for (int i = 0; i < _peoples.Count; i++) {
//                _peoples[i].FixedTick();
//            }

            if (DateTime.Now.Ticks % 1000 == 0) {
                for (int i = 0; i < _settings.startingSpawns; i++) {
                    SpawnNext();
                }
            }
        }

        public int PeoplesCount => _peoples.Count;

        [Serializable]
        public class Settings {
            public int startingSpawns;
        }
    }
}