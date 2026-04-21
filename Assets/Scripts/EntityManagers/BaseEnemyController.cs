using UnityEngine;

using AbilitySystem;

using System.Collections.Generic;
using System.Linq;

public class BaseEnemyController : MonoBehaviour {
    [Space]
    [Header("Script Managers")]
    [SerializeField] SingleAbilityEnemyController AbilityController;

    [Space]
    [Header("Settings")]
    [SerializeField] float playerStopDistance = 1;
    [SerializeField] float playerAttackDistance = 2;
    [SerializeField] float playerChaseDistance = 10;
    List<AbilityController> opposingTeam = new();
    private Vector3 location;
    private float distanceToEntity {
        get => Vector3.Distance(transform.position, EntityManager.instance != null ? location : PlayerManager.player.transform.position);
    }

    void Awake() {
        AbilityController = GetComponent<SingleAbilityEnemyController>();
    }

    void Start() {
        AbilityController.GetInputValues.SetMovementTypeToggle(MovementType.AutoTrack);
        opposingTeam = EntityManager.instance?.GetOpposingTeam(AbilityController.entityTeam);
        AbilityController.GetInputValues.SetDestination(gameObject.transform.position);
        //foreach (var o in opposingTeam) Debug.Log(o.gameObject.name);
    }

    public void Update() {
        if (EntityManager.instance != null) {
            location =
                opposingTeam.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .First()
                .transform.position;
        }

        if (distanceToEntity > playerChaseDistance) return;
        if (distanceToEntity > playerStopDistance) {
            Debug.Log("AAAA");
            AbilityController.GetInputValues.SetDestination(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.GetInputValues.isPrimaryAbility = false;
        }
        if (distanceToEntity <= playerAttackDistance)
            AbilityController.GetInputValues.isPrimaryAbility = true;

    }
}
