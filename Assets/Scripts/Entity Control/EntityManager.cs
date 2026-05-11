using System.Collections.Generic;
using System.Linq;

using AbilitySystem;

using UnityEngine;

using UnityEditor;

public class EntityManager : MonoBehaviour {
    public static EntityManager instance { get; private set; }
    public List<AbilityController> entities;

    void Awake() {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        entities = FindObjectsByType<AbilityController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }
    public void ActivateAllEntities() {
        foreach (var n in entities)
            n.OnEnable();
    }
    public void DeactivateAllEntities() {
        foreach (var n in entities)
            n.OnDisable();
    }
    public void GetAbilityControllers() => entities = FindObjectsByType<AbilityController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    public List<AbilityController> GetOpposingTeam(EntityTeam currentTeam) {
        List<AbilityController> abilityControllers = new();
        foreach (var e in entities) {
            if (!EntityTeamFunctions.HasCommonTeam(e.entityTeam, currentTeam))
                abilityControllers.Add(e);
        }
        return abilityControllers;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EntityManager))]
[CanEditMultipleObjects]
public class EntityManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        EntityManager entityManager = target as EntityManager;
        if (GUILayout.Button("Locate Ability Controllers")) {
            entityManager.GetAbilityControllers();
        }
    }
}
#endif