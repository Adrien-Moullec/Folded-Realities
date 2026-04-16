using System;

using UnityEngine;

using UnityEditor;

[Serializable]
public class AreaColliderCheck {
    public CheckShape checkShape;
    public bool DrawGizmo = false;
    public Vector3 centerOffset = Vector3.zero;
    public float size1 = 1;
    public Vector3 halfExtents = Vector3.one;
    public LayerMask layers = 1;

    public Action<RaycastHit[]> GetColliders(GameObject gameObject) => GetColliders(gameObject.transform.position, gameObject.transform.forward);

    public Action<RaycastHit[]> GetColliders(Vector3 location, Vector3 direction) {
        return checkShape switch {
            CheckShape.Sphere => (RaycastHit[] x) => {
                Physics.SphereCastNonAlloc(location + centerOffset, size1, direction, x, size1, layers);
            }
            ,
            CheckShape.Cube => (RaycastHit[] x) => {
                Physics.BoxCastNonAlloc(location + centerOffset, halfExtents, direction, x, Quaternion.Euler(direction), Mathf.Max(halfExtents.y, halfExtents.z), layers);
            }
            ,
            _ => null,
        };
    }
    public void Gizmo(GameObject gameObject) => Gizmo(gameObject.transform.position, gameObject.transform.forward);
    public void Gizmo(Vector3 location, Vector3 direction) {
        if (!DrawGizmo) return;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Gizmos.matrix = Matrix4x4.TRS(location + rotation * centerOffset, rotation, Vector3.one);
        Gizmos.color = Color.red;
        switch (checkShape) {
            case CheckShape.Sphere:
                Gizmos.DrawWireSphere(Vector3.zero, size1);
                break;

            case CheckShape.Cube:
                Gizmos.DrawWireCube(Vector3.zero, halfExtents);
                break;
        }
        Gizmos.matrix = Matrix4x4.identity;
    }
}

[CustomPropertyDrawer(typeof(AreaColliderCheck))]
public class AreaAffectsDrawer : PropertyDrawer {
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

            DrawProp(ref y, position, property, "checkShape");
            DrawProp(ref y, position, property, "centerOffset");

            SerializedProperty shapeProp = property.FindPropertyRelative("checkShape");
            CheckShape shape = (CheckShape)shapeProp.enumValueIndex;

            if (shape == CheckShape.Sphere)
                DrawProp(ref y, position, property, "size1");
            else if (shape == CheckShape.Cube)
                DrawProp(ref y, position, property, "halfExtents");

            DrawProp(ref y, position, property, "layers");
            DrawProp(ref y, position, property, "DrawGizmo");

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }

    void DrawProp(ref float y, Rect position, SerializedProperty property, string name) {
        SerializedProperty prop = property.FindPropertyRelative(name);

        EditorGUI.PropertyField(
            new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
            prop
        );

        y += EditorGUIUtility.singleLineHeight + 2;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        int lines = 4;

        SerializedProperty shapeProp = property.FindPropertyRelative("checkShape");
        CheckShape shape = (CheckShape)shapeProp.enumValueIndex;

        if (shape == CheckShape.Sphere) lines++;
        if (shape == CheckShape.Cube) lines++;

        lines++;

        return (lines + 1) * (EditorGUIUtility.singleLineHeight + 2);
    }
}

public enum CheckShape {
    Cube,
    Sphere
}