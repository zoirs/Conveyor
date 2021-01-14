// using System;

using System;
using System.Collections.Generic;
using Zenject;
using Random = UnityEngine.Random;

public class TaskManager : IDisposable, IInitializable {
    
    [Inject] private SignalBus _signalBus;
    [Inject] private CityManager _cityManager;
    [Inject] private GameSettingsInstaller.TaskSetting _taskSetting;

    private List<Task> currentTasks = new List<Task>();

    public void Initialize() {
        _signalBus.Subscribe<TaskCompleteSignal>(OnTaskComplete);
    }

    public void Dispose() {
        _signalBus.Unsubscribe<TaskCompleteSignal>(OnTaskComplete);
    }

    private void OnTaskComplete() {
        Task task = currentTasks.Find(t => t.Customer == null);
        _cityManager.AddTask(task);
    }

    public void Start() {
        for (int i = 0; i < _taskSetting.brownTaskCount; i++) {
            int resourceCount = Random.Range(_taskSetting.brownResourceCountFrom, _taskSetting.brownResourceCountTo);
            int price = Random.Range(_taskSetting.brownResourcePriceFrom, _taskSetting.brownResourcePriceTo);
            currentTasks.Add(new Task(ProductType.Brown, resourceCount, price));
        }
        for (int i = 0; i < _taskSetting.yellowTaskCount; i++) {
            int resourceCount = Random.Range(_taskSetting.yellowResourceCountFrom, _taskSetting.yellowResourceCountTo);
            int price = Random.Range(_taskSetting.yellowResourcePriceFrom, _taskSetting.yellowResourcePriceTo);
            currentTasks.Add(new Task(ProductType.Yellow, resourceCount, price));
        }

        foreach (Task task in currentTasks) {
            _cityManager.AddTask(task);
        }
    }

    public List<Task> CurrentTasks => currentTasks;
}