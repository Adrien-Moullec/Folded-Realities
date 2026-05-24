using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class OrigamiTimeline : MonoBehaviour {
    [SerializeField] public bool playOnAwake = false;
    [SerializeField] public UnityEvent onStart;
    [SerializeField] public UnityEvent timelineStartEvents;
    [SerializeField] public UnityEvent timelineEndEvents;

    private PlayableDirector playableDirector;

    private void Awake() {
        playableDirector = GetComponent<PlayableDirector>();
        onStart.Invoke();
        playableDirector.played += input => {
            timelineStartEvents.Invoke();
            EntityManager.instance.DeactivateAllEntities();
            if (TryGetComponent(out Collider c)) c.enabled = false;
        };
        playableDirector.stopped += input => {
            timelineEndEvents.Invoke();
            EntityManager.instance.ActivateAllEntities();
        };
        if (playOnAwake) playableDirector.Play();
    }

    void OnDisable() {
        playableDirector.played -= input => {
            timelineStartEvents.Invoke();
            EntityManager.instance.DeactivateAllEntities();
            if (TryGetComponent(out Collider c)) c.enabled = false;
        };
        playableDirector.stopped -= input => {
            timelineEndEvents.Invoke();
            EntityManager.instance.ActivateAllEntities();
        };
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag != "Player") return;
        playableDirector.Play();
    }

    void OnValidate() {
        if (playableDirector == null) playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playOnAwake = false;
    }
}
