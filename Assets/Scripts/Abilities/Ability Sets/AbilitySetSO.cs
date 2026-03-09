using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AbilitySystem
{
    [Serializable]
    public abstract class AbilitySet
    {
        [SerializeField] internal string abilitySetName;
        [SerializeField] internal MovementAbilitySummary movement;

        public AbilitySet(string name, MovementSO movementSO)
        {
            abilitySetName = name;

            if (movementSO != null)
                movement = new(movementSO);
        }
        public AbilitySet(AbilitySetSO abilitySet)
        {
            abilitySetName = abilitySet.abilitySetName;

            if (abilitySet.movement != null)
                movement = new MovementAbilitySummary(abilitySet.movement);
        }
    }
    public abstract class AbilitySetSO : ScriptableObject
    {
        [SerializeField] internal string abilitySetName;
        [SerializeField] internal MovementSO movement;

        internal virtual void SetupAnimations(Animation anim)
        {
            if (anim == null) return;
            if (movement != null) AssignAnimations(anim, movement);
        }
        protected static void AssignAnimations(Animation anim, AbilitySO ability)
        {
            AnimationClip clip;
            foreach (var clipInfo in ability.AbilityAnimationsSetup())
            {
                if (clipInfo.animation != null)
                {
                    clip = clipInfo.animation;
                    clip.legacy = true;
                    anim.AddClip(clip, clipInfo.clipName);
                    anim[clipInfo.clipName].wrapMode = clipInfo.wrapMode;
                    anim[clipInfo.clipName].speed = clipInfo.speed;
                }
            }
        }
    }

    #region Editor
    // Editor for ScriptBase and all derived types
    [CustomEditor(typeof(AbilitySetSO), true)]
    public class AbilitySetSOEditor : Editor
    {
        private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        private void OnEnable()
        {
            SerializedProperty prop = serializedObject.GetIterator();
            while (prop.NextVisible(true))
            {
                if (!foldouts.ContainsKey(prop.propertyPath))
                    foldouts[prop.propertyPath] = false;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawNestedScriptableObjects();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNestedScriptableObjects()
        {
            SerializedProperty prop = serializedObject.GetIterator();
            bool enterChildren = true;

            while (prop.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (prop.name == "m_Script")
                    continue;

                EditorGUILayout.PropertyField(prop, true);

                if (prop.propertyType == SerializedPropertyType.ObjectReference &&
                    prop.objectReferenceValue is ScriptableObject nestedSO &&
                    nestedSO != null)
                {
                    foldouts[prop.propertyPath] = EditorGUILayout.Foldout(
                        foldouts[prop.propertyPath],
                        "",
                        true
                    );

                    EditorGUI.indentLevel++;

                    if (foldouts[prop.propertyPath])
                    {
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
    #endregion
}