using System;
using DefaultNamespace;
using Zenject;

namespace Main {
    public enum GameStates {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable {
        readonly PeopleManager _peopleManager;

        GameStates _state = GameStates.WaitingToStart;

        public GameController(PeopleManager peopleManager) {
            _peopleManager = peopleManager;
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

            _peopleManager.Start();
            _state = GameStates.Playing;
        }
    }
}