using System;
using System.Collections.Generic;

using AbilitySystem;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class OrigamiTimeline : MonoBehaviour, IHealth, IInteractable {
    [Header("Play Options")]
    [SerializeField] public bool playOnAwake = false;
    [SerializeField] public bool playOnTrigger = false;
    [SerializeField] public bool playOnInteract = false;
    [SerializeField] public bool playOnDamage = false;

    [Space]
    [Header("Play Options")]
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
        if (other.tag != "Player" || !playOnTrigger) return;
        playableDirector.Play();
    }

    void OnValidate() {
        if (playableDirector == null) playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playOnAwake = false;
    }

    public void Damage(EntityDamage damage) {
        if (damage.damagingTeam == EntityTeam.Player && playOnDamage) {
            playableDirector.Play();
        }
    }

    public void Heal(EntityDamage heal) { }

    public void Die() { }

    public void SetMaxHealth() { }

    public void OnInteract() {
        if (playOnInteract)
            playableDirector.Play();
    }

    public void OnCancelInteract() { }
}
