using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AbilitySystem {
    [Serializable]
    public abstract class AbilitySet {
        [Space]
        [Header("Ability Options")]
        [SerializeField] public string abilitySetName;
        [SerializeField] public HealthSO healthSettings;
        [SerializeField] public MovementAbilitySummary movement;

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
    public abstract class AbilitySetSO : ScriptableObject {
        [SerializeField] public string abilitySetName;
        [SerializeField] public HealthSO healthSettings;
        [SerializeField] public MovementSO movement;
    }

    #region Editor
#if UNITY_EDITOR
    // Editor for ScriptBase and all derived types
    [CustomEditor(typeof(AbilitySetSO), true)]
    public class AbilitySetSOEditor : Editor {
        private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        private void OnEnable() {
            SerializedProperty prop = serializedObject.GetIterator();
            while (prop.NextVisible(true)) {
                if (!foldouts.ContainsKey(prop.propertyPath))
                    foldouts[prop.propertyPath] = false;
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawNestedScriptableObjects();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNestedScriptableObjects() {
            SerializedProperty prop = serializedObject.GetIterator();
            bool enterChildren = true;

            while (prop.NextVisible(enterChildren)) {
                enterChildren = false;

                if (prop.name == "m_Script")
                    continue;

                EditorGUILayout.PropertyField(prop, true);

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