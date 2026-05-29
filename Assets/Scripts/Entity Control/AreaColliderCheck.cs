using System;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Area collider check is a custom variable that allows checks for colliders without needing a physical gameobject in the scene.
/// </summary>
[Serializable]
public class AreaColliderCheck {
    [Tooltip("Type of shade the 'collider' searches in.")]
    public CheckShape checkShape;
    [Tooltip("Offset to shape-check from.")]
    public Vector3 centerOffset = Vector3.zero;
    [Tooltip("Sphere radius to check.")]
    public float size1 = 1;
    [Tooltip("Cube half-extents to check.")]
    public Vector3 halfExtents = Vector3.one;
    [Tooltip("Layers to check for colliders.")]
    public LayerMask layers = 1;
    [Tooltip("Draw Gizmo for editor sake.")]
    public bool doDrawGizmo = true;
    [Tooltip("Wireframe draw of the gizmo.")]
    public bool wireFrame = true;

    /// <summary>
    /// Return number of collisions and set list of ref Collider[] with found colliders
    /// </summary>
    public Func<Collider[], int> GetColliders(GameObject gameObject) => GetColliders(gameObject.transform.position, gameObject.transform.forward);
    public Func<Collider[], int> GetColliders(Transform trans) => GetColliders(trans.position, trans.forward);
    public Func<Collider[], int> GetColliders(Vector3 location, Vector3 direction) {
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 pos = location + rotation * centerOffset;

        /// Check areas based on input shape.
        return checkShape switch {
            CheckShape.Sphere => (Collider[] x) => Physics.OverlapSphereNonAlloc(pos, size1, x, layers),
            CheckShape.Cube => (Collider[] x) => Physics.OverlapBoxNonAlloc(pos, halfExtents, x, rotation, layers),
            _ => null,
        };
    }

    /// <summary>
    /// Get ray collision instead of shape area checks
    /// </summary>
    public static Func<RaycastHit[], int> GetRayCastColliders(Vector3 location, Vector3 direction, LayerMask layerMask) {
        Quaternion rotation = Quaternion.LookRotation(direction);
        Ray ray = new Ray(location, direction);
        return (RaycastHit[] x) => Physics.RaycastNonAlloc(ray, x, 1, layerMask);
    }

#if UNITY_EDITOR

    /// <summary>
    /// Draw Gizmo based on gameobject/transform/location and direction for editor testing.
    /// </summary>
    public void Gizmo(GameObject gameObject) => Gizmo(gameObject.transform);
    public void Gizmo(Transform trans) => Gizmo(trans.position, trans.forward);
    public void Gizmo(Vector3 location, Vector3 direction) {
        if (!doDrawGizmo) return;
        Quaternion rotation = Quaternion.LookRotation(direction);

        /// Rotate matrix to match collision shape area.
        Gizmos.matrix = Matrix4x4.TRS(location + rotation * centerOffset, rotation, Vector3.one);
        Gizmos.color = Color.red;

        /// Draw shape area.
        switch (checkShape) {
            case CheckShape.Sphere:
                if (wireFrame) Gizmos.DrawWireSphere(Vector3.zero, size1);
                else Gizmos.DrawSphere(Vector3.zero, size1);
                break;

            case CheckShape.Cube:
                if (wireFrame) Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
                else Gizmos.DrawCube(Vector3.zero, halfExtents * 2f);
                break;
        }
        /// Reset Matrix
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}

#if UNITY_EDITOR
/// <summary>
/// A custom display of AreaColliderCheck
/// </summary>
[CustomPropertyDrawer(typeof(AreaColliderCheck))]
public class AreaAffectsDrawer : PropertyDrawer {
    /// <summary>
    /// Display properties of AreaColliderCheck
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        /// Expand property if set to.
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            label
        );

        if (property.isExpanded) {
            EditorGUI.indentLevel++;

            /// Draw properties for shape and offset.
            float y = position.y + EditorGUIUtility.singleLineHeight;
            DrawProp(ref y, position, property, "checkShape");
            DrawProp(ref y, position, property, "centerOffset");

            /// Get the value of CheckShape to determine the next displayed property.
            SerializedProperty shapeProp = property.FindPropertyRelative("checkShape");
            CheckShape shape = (CheckShape)shapeProp.enumValueIndex;

            if (shape == CheckShape.Sphere)
                DrawProp(ref y, position, property, "size1");
            else if (shape == CheckShape.Cube)
                DrawProp(ref y, position, property, "halfExtents");

            /// Draw the rest of the properties
            DrawProp(ref y, position, property, "layers");
            DrawProp(ref y, position, property, "doDrawGizmo");
            DrawProp(ref y, position, property, "wireFrame");

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }

    /// <summary>
    /// Draw property and drop the y position by a line height amount
    /// </summary>
    void DrawProp(ref float y, Rect position, SerializedProperty property, string name) {
        SerializedProperty prop = property.FindPropertyRelative(name);

        EditorGUI.PropertyField(
            new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
            prop
        );

        y += EditorGUIUtility.singleLineHeight + 2;
    }

    /// <summary>
    /// Set the variable height it takes up by what variables are currently selected. 
    /// </summary>
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
#endif

/// <summary>
/// Check shape options
/// </summary>
public enum CheckShape {
    Cube,
    Sphere,
    Ray
}