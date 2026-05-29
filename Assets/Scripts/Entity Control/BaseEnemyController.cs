using UnityEngine;

using AbilitySystem;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Enemy manager for communicating with the ability controller by setting movement and ability logic.
/// </summary>
public class BaseEnemyController : MonoBehaviour {
    [Space]
    [Header("Script Managers")]
    [Tooltip("Ability controller.")]
    [SerializeField] SingleAbilityEnemyController AbilityController;
    [Tooltip("Editor option for debugging the script.")]
    [SerializeField] bool DebugEnemy = false;

    [Space]
    [Header("Settings")]
    [Tooltip("Distance to the player before the enemy stops.")]
    [SerializeField] float playerStopDistance = 1;
    [Tooltip("Distance to the player before the enemy attacks.")]
    [SerializeField] float playerAttackDistance = 2;
    [Tooltip("Distance to the player before the enemy attacks with 2nd attack.")]
    [SerializeField] float playerAttack2Distance = 8;
    [Tooltip("Distance to the player before the enemy starts chasing the player.")]
    [SerializeField] float playerChaseDistance = 10;

    [Tooltip("List of ability controllers on the opposing team to target.")]
    List<AbilityController> opposingTeam = new();
    [Tooltip("Target location.")]
    private Vector3 location;
    [Tooltip("Start position so the enemy moves back after player goes out of bounds.")]
    Vector3 startPos = Vector3.zero;
    [Tooltip("Returns the distance to target entity.")]
    private float distanceToEntity {
        get => Vector3.Distance(transform.position, EntityManager.instance != null ? location : PlayerManager.player.transform.position);
    }

    /// <summary>
    /// Setup components and values
    /// </summary>
    void Awake() {
        AbilityController = GetComponent<SingleAbilityEnemyController>();
        startPos = transform.position;
    }

    /// <summary>
    /// Setup abilitydata movement, inputs and entity teams
    /// </summary>
    void Start() {
        AbilityController.GetInputValues.SetMovementTypeToggle(MovementType.AutoTrack);
        opposingTeam = EntityManager.instance?.GetOpposingTeam(AbilityController.entityTeam);
        AbilityController.GetInputValues.SetDestination(gameObject.transform.position);
    }

    /// <summary>
    /// Check next move position every frame.
    /// </summary>
    public void Update() {

        /// Find the next location
        if (EntityManager.instance != null) {
            location = opposingTeam.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .First().transform.position;
        }

        /// Chase logic
        if (distanceToEntity > playerChaseDistance) {
            if (DebugEnemy) Debug.Log("TOO FAR AWAY");
            AbilityController.OnMoveEntity(startPos);
            AbilityController.GetInputValues.isSecondaryAbility = false;
            AbilityController.GetInputValues.isPrimaryAbility = false;
            return;
        }
        /// Stop logic
        if (distanceToEntity > playerStopDistance) {
            if (DebugEnemy) Debug.Log("RUN AT PLAYER");
            //AbilityController.GetInputValues.SetDestination(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.OnMoveEntity(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.GetInputValues.isPrimaryAbility = true;
            AbilityController.GetInputValues.isSecondaryAbility = false;
        }
        /// Fight with attack 1 call
        if (distanceToEntity <= playerAttackDistance) {
            if (DebugEnemy) Debug.Log("ATTTACK");
            AbilityController.GetInputValues.isPrimaryAbility = true;
            AbilityController.GetInputValues.isSecondaryAbility = false;
        }
        /// Fight with attack 2 call
        if (distanceToEntity < playerChaseDistance && distanceToEntity > playerAttack2Distance) {
            if (DebugEnemy) Debug.Log("Attack 2");
            AbilityController.OnMoveEntity(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.GetInputValues.isSecondaryAbility = true;
            AbilityController.GetInputValues.isPrimaryAbility = false;
        }

    }
}