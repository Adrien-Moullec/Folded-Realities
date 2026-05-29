using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AbilitySystem {

    /// <summary>
    /// Base ability set script to hold entity ability data. Has by default health and movement options.
    /// </summary>
    [Serializable]
    public abstract class AbilitySet {
        [Space]
        [Header("Ability Options")]
        [Tooltip("Name of the ability set.")]
        [SerializeField] public string abilitySetName;
        [Tooltip("Health settings SO Ability.")]
        [SerializeField] public HealthSO healthSettings;
        [Tooltip("Movement settings SO Ability.")]
        [SerializeField] public MovementAbilitySummary movement;

        /// <summary>
        /// Unused function for AI
        /// </summary>
        public virtual void ExecuteBestAbility() { }

        /// <summary>
        /// Base setup for ability set.
        /// </summary>
        public AbilitySet(string name, MovementSO movementSO, HealthSO healthSO, EntityBody eb) {
            abilitySetName = name;

            if (movementSO != null)
                movement = new(movementSO, eb);
            if (healthSO != null)
                healthSettings = healthSO;
        }
        public AbilitySet(AbilitySetSO abilitySet, EntityBody eb) {
            abilitySetName = abilitySet.abilitySetName;
            healthSettings = abilitySet.healthSettings;

            if (abilitySet.movement != null)
                movement = new MovementAbilitySummary(abilitySet.movement, eb);

        }
    }
    /// <summary>
    /// Ability set scriptable object to hold ability data.
    /// </summary>
    public abstract class AbilitySetSO : ScriptableObject {
        [SerializeField] public string abilitySetName;
        [SerializeField] public HealthSO healthSettings;
        [SerializeField] public MovementSO movement;
    }

    #region Editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor for every ability set's derived types abilities. Displays editable data for each set.
    /// </summary>
    [CustomEditor(typeof(AbilitySetSO), true)]
    public class AbilitySetSOEditor : Editor {

        /// Foldout data
        private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        /// <summary>
        /// Loop through properties while there are still some to check for foldout information.
        /// </summary>
        private void OnEnable() {
            SerializedProperty prop = serializedObject.GetIterator();
            while (prop.NextVisible(true)) {
                if (!foldouts.ContainsKey(prop.propertyPath))
                    foldouts[prop.propertyPath] = false;
            }
        }

        /// <summary>
        /// Draw Ability Set.
        /// </summary>
        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawNestedScriptableObjects();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Loop through and display ability SOs.
        /// </summary>
        private void DrawNestedScriptableObjects() {
            SerializedProperty prop = serializedObject.GetIterator();
            bool enterChildren = true;

            /// Draw every property
            while (prop.NextVisible(enterChildren)) {
                enterChildren = false;

                if (prop.name == "m_Script")
                    continue;

                /// Draw property
                EditorGUILayout.PropertyField(prop, true);

                /// If next prop is scriptable object, draw the editor of that scriptable object.
                if (prop.propertyType == SerializedPropertyType.ObjectReference &&
                    prop.objectReferenceValue is ScriptableObject nestedSO &&
                    nestedSO != null) {
                    foldouts[prop.propertyPath] = EditorGUILayout.Foldout(
                        foldouts[prop.propertyPath],
                        "",
                        true
                    );

                    EditorGUI.indentLevel++;

                    if (foldouts[prop.propertyPath]) {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField(prop.displayName, EditorStyles.boldLabel);

                        Editor nestedEditor = CreateEditor(nestedSO);
                        if (nestedEditor != null)
                            nestedEditor.OnInspectorGUI();

                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.Space();

                    EditorGUI.indentLevel--;
                }
            }
        }
    }
#endif
    #endregion
}