using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class OrigamiTimeline : MonoBehaviour {
    private PlayableDirector playableDirector;
    [SerializeField] public UnityEvent onStart;
    [SerializeField] public UnityEvent timelineStartEvents;
    [SerializeField] public UnityEvent timelineEndEvents;
    private void Start() {
        playableDirector = GetComponent<PlayableDirector>();
        onStart.Invoke();
        playableDirector.played += input => {
            timelineStartEvents.Invoke();
            EntityManager.instance.DeactivateAllEntities();
        };
        playableDirector.stopped += input => {
            timelineEndEvents.Invoke();
            EntityManager.instance.ActivateAllEntities();
            GetComponent<Collider>().enabled = false;
            Debug.Log("END");
        };
    }
    void OnDisable() {
        playableDirector.played -= input => timelineStartEvents.Invoke();
        playableDirector.stopped -= input => timelineEndEvents.Invoke();
    }
    public void OnTriggerEnter(Collider other) {
        if (other.tag != "Player") return;
        playableDirector.Play();
    }
}
