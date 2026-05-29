using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// Experimental script for setting timeline events that pass through float values, the original concept for setting predetermined falling gamemode gameplay.
/// </summary>

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