using System;
using System.ComponentModel;
using DefaultNamespace;
using Line;
using Zenject;

namespace Main {
    public enum GameStates {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable {
        // readonly PeopleManager _peopleManager;
        readonly LineManager _lineManager;
        readonly TaskManager _taskManager;

        GameStates _state = GameStates.WaitingToStart;

        public GameController(LineManager lineManager, TaskManager taskManager) {
            // _peopleManager = peopleManager;
            _lineManager = lineManager;
            _taskManager = taskManager;
        }

        public void Initialize() { }

        public void Dispose() { }

        public void Tick() {
            switch (_state) {
                case GameStates.WaitingToStart: {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing: {
//                UpdatePlaying();
                    break;
                }
                case GameStates.GameOver: {
//                UpdateGameOver();
                    break;
                }
                default: {
//                Assert.That(false);
                    break;
                }
            }
        }

        private void UpdateStarting() {
//         Assert.That(_state == GameStates.WaitingToStart);

//            _peopleManager.Start();
        }

        public void StartGame() {
            _lineManager.Start();
            _taskManager.Start();
            _state = GameStates.Playing;
        }

        public GameStates State => _state;
    }
}