using System;
using System.Collections.Generic;

using AbilitySystem;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

/// <summary>
/// Timeline manager script that allows events to be set during the scene start then the start and end of animation.
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class OrigamiTimeline : MonoBehaviour, IHealth, IInteractable {
    [Header("Play Options")]
    [Tooltip("Play on awake option")]
    [SerializeField] public bool playOnAwake = false;
    [Tooltip("Play on trigger area option")]
    [SerializeField] public bool playOnTrigger = false;
    [Tooltip("Play on interact with area option")]
    [SerializeField] public bool playOnInteract = false;
    [Tooltip("Play on damage area option")]
    [SerializeField] public bool playOnDamage = false;
    [Tooltip("Damage type for activation of timeline.")]
    [SerializeField] public EntityDamageType damageActivateType = EntityDamageType.Normal;

    [Space]
    [Header("Play Events")]
    [Tooltip("Events at scene start.")]
    [SerializeField] public UnityEvent onStart;
    [Tooltip("Events at timeline start.")]
    [SerializeField] public UnityEvent timelineStartEvents;
    [Tooltip("Events at timeline end.")]
    [SerializeField] public UnityEvent timelineEndEvents;

    [Tooltip("PlayableDirector timeline reference.")]
    private PlayableDirector playableDirector;

    /// <summary>
    /// Setup events and timeline references, then try play on awake.
    /// </summary>
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

    /// <summary>
    /// Disable events on Disable
    /// </summary>
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

    /// <summary>
    /// Try activate timeline on player enter.
    /// </summary>
    /// <param name="other"> Entering collider </param>
    public void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || !playOnTrigger) return;
        playableDirector.Play();
    }

    /// <summary>
    /// Turn off play on awake on PlayableDirector for better custom setup.
    /// </summary>
    void OnValidate() {
        if (playableDirector == null) playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playOnAwake = false;
    }

    /// <summary>
    /// Try activate timeline on damage.
    /// </summary>
    public void Damage(EntityDamage damage) {
        if (damage.damagingTeam == EntityTeam.Player && playOnDamage && damageActivateType == damage.type) {
            playableDirector.Play();
        }
    }

    /// <summary>
    /// Try activate timeline on interact.
    /// </summary>
    public void OnInteract() {
        if (playOnInteract)
            playableDirector.Play();
    }

    #region Unused
    public void Heal(EntityDamage heal) { }
    public void Die() { }
    public void SetMaxHealth() { }
    public void OnCancelInteract() { }
    #endregion

}
