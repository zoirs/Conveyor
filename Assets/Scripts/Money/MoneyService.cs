using System;
using Zenject;

public class MoneyService : IDisposable, IInitializable {
    public int balance;
    
    [Inject] private SignalBus _signalBus;

    public void Dispose() {
        _signalBus.Unsubscribe<AddProductSignal>(OnEnterToStation);
    }

    public void Initialize() {
        _signalBus.Subscribe<AddProductSignal>(OnEnterToStation);
    }

    private void OnEnterToStation() {
        // balance = balance + 5;
    }
    
    public void Minus(int money) {
        balance = balance - money;
    }

    public void Plus(int money) {
        balance = balance + money;
    }

    [Serializable]
    public class PriceSettings {
        public int train;
        public int trainWagon;
        public int trainSpeed;
    }
}