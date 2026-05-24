using System;

using UnityEngine;

using UnityEditor;

[Serializable]
public class TargetLevel {
    public GameplayScenes targetScene;
    public BedroomSpawnPoints bedroomSpawnPoint;
}

[CustomPropertyDrawer(typeof(TargetLevel))]
public class TargetLevelDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            label
        );

        if (property.isExpanded) {
            EditorGUI.indentLevel++;
            float y = position.y + EditorGUIUtility.singleLineHeight;

            DrawProp(ref y, position, property, "targetScene");
            GameplayScenes scene = (GameplayScenes)property.FindPropertyRelative("targetScene").enumValueIndex;

            switch (scene) {
                case GameplayScenes.MainMenu: break;
                case GameplayScenes.Bedroom: DrawProp(ref y, position, property, "bedroomSpawnPoints"); break;
                case GameplayScenes.IntroCutscene: break;
                case GameplayScenes.Tutorial2: break;
            }

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }

    void DrawProp(ref float y, Rect position, SerializedProperty property, string name) {
        SerializedProperty prop = property.FindPropertyRelative(name);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), prop);
        y += EditorGUIUtility.singleLineHeight + 2;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        return 4 * (EditorGUIUtility.singleLineHeight + 2);
    }
}