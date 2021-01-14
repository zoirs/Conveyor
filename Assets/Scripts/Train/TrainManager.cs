using System.Collections.Generic;
using Zenject;

namespace Train {
    public class TrainManager {
        [Inject] private TrainController.Factory _factoryTrain;
        private List<TrainController> trains = new List<TrainController>();

        public void Create() {
            TrainController trainController = _factoryTrain.Create();
            trains.Add(trainController);
        }

        public List<TrainController> Trains => trains;
    }
}