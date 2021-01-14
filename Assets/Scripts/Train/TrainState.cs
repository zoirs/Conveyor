using System;

namespace Train {
    public enum TrainState {
        Depo,
        WaitingSetLine,
        Going,
        OnStation,
        OnTurn
    }

    static class TrainStateExtensions {
        public static string Text(this TrainState key) {
            switch (key) {
                case TrainState.Depo:
                    return "В депо";
                case TrainState.WaitingSetLine:
                    return "Выходит на линию";
                case TrainState.Going:
                    return "В пути";
                case TrainState.OnStation:
                    return "В пути";
                case TrainState.OnTurn:
                    return "В пути";
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }
    }
}