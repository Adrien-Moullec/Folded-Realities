using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

/*
[CreateAssetMenu(menuName = "Signals/Float Signal")]
public class FloatSignal : ScriptableObject {
    public UnityEvent<float> OnSignalRaised;

    public void Raise(float value) {
        OnSignalRaised?.Invoke(value);
    }
}*/
public class FloatSignalEmitter : SignalEmitter {
    public float value;
}
public class Float01SignalEmitter : SignalEmitter {
    [Range(0, 1)] public float value;
}