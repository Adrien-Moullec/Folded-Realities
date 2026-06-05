using System.Collections.Generic;
using System.Linq;

using AbilitySystem;

using UnityEngine;

using UnityEditor;

/// <summary>
/// Summary of entities controlled by an ability controller in a level for easy access of information.
/// </summary>
public class EntityManager : MonoBehaviour {
    [Tooltip("Sole instance of an EntityManager in a level.")]
    public static EntityManager instance { get; private set; }
    [Tooltip("List of all ability controller entities in a level.")]
    public List<AbilityController> entities;

    /// <summary>
    /// Create Singleton
    /// </summary>
    void Awake() {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        entities = FindObjectsByType<AbilityController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    /// <summary>
    /// Activate all ability controllers.
    /// </summary>
    public void ActivateAllEntities() {
        Awake();
        foreach (var n in entities)
            n.OnEnable();
    }
    /// <summary>
    /// Deactivate all ability controllers.
    /// </summary>
    public void DeactivateAllEntities() {
        Awake();
        foreach (var n in entities)
            n.OnDisable();
    }
    /// <summary>
    /// Find all ability controllers in the scene.
    /// </summary>
    public void GetAbilityControllers() => entities = FindObjectsByType<AbilityController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

    /// <summary>
    /// Get ability controllers of the opposite team from a defined team.
    /// </summary>
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
/// <summary>
/// Custom editor for checking for ability controllers.
/// </summary>
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