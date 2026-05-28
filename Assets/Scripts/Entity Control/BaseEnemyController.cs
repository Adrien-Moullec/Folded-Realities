using UnityEngine;

using AbilitySystem;

using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class BaseEnemyController : MonoBehaviour {
    [Space]
    [Header("Script Managers")]
    [SerializeField] SingleAbilityEnemyController AbilityController;
    [SerializeField] bool DebugEnemy = false;

    [Space]
    [Header("Settings")]
    [SerializeField] float playerStopDistance = 1;
    [SerializeField] float playerAttackDistance = 2;
    [SerializeField] float playerChaseDistance = 10;
    List<AbilityController> opposingTeam = new();
    private Vector3 location;
    Vector3 startPos = Vector3.zero;
    private float distanceToEntity {
        get => Vector3.Distance(transform.position, EntityManager.instance != null ? location : PlayerManager.player.transform.position);
    }

    void Awake() {
        AbilityController = GetComponent<SingleAbilityEnemyController>();
        startPos = transform.position;
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

        if (distanceToEntity > playerChaseDistance) {
            if (DebugEnemy) Debug.Log("TOO FAR AWAY");
            AbilityController.OnMoveEntity(startPos);
            return;
        }
        if (distanceToEntity > playerStopDistance) {
            if (DebugEnemy) Debug.Log("RUN AT PLAYER");
            //AbilityController.GetInputValues.SetDestination(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.OnMoveEntity(EntityManager.instance != null ? location : PlayerManager.player.transform.position);
            AbilityController.GetInputValues.isPrimaryAbility = false;
        }
        if (distanceToEntity <= playerAttackDistance) {
            if (DebugEnemy) Debug.Log("ATTTACK");
            AbilityController.GetInputValues.isPrimaryAbility = true;
        }

    }
}