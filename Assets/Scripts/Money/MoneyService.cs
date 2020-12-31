using System;
using Zenject;

public class MoneyService : IDisposable, IInitializable {
    public int balance;
    
    [Inject] private SignalBus _signalBus;

    public void Dispose() {
        _signalBus.Unsubscribe<EnterToStationSignal>(OnEnterToStation);
    }

    public void Initialize() {
        _signalBus.Subscribe<EnterToStationSignal>(OnEnterToStation);
    }

    private void OnEnterToStation() {
        balance = balance + 5;
    }
    
    public void Minus(int money) {
        balance = balance - money;
    }

    [Serializable]
    public class PriceSettings {
        public int station;
        public int lineElement;
        public int ticket;
    }
}