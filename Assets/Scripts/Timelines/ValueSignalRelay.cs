using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(SignalReceiver))]
public class ValueSignalRelay : MonoBehaviour, INotificationReceiver {
    [SerializeField] UnityEvent<float> FloatEvent;
    public void OnNotify(Playable origin, INotification notification, object context) {
        if (notification is FloatSignalEmitter emitter)
            FloatEvent.Invoke(emitter.value);
        else if (notification is Float01SignalEmitter emitter01)
            FloatEvent.Invoke(emitter01.value);
    }
}