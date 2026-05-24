using System;

using UnityEngine;

using UnityEditor;

[Serializable]
public class AreaColliderCheck {
    public CheckShape checkShape;
    public Vector3 centerOffset = Vector3.zero;
    public float size1 = 1;
    public Vector3 halfExtents = Vector3.one;
    public LayerMask layers = 1;
    public bool doDrawGizmo = true;
    public bool wireFrame = true;

    // int s = wallCheckArea.GetColliders(entityBody.bodyHolder).Invoke(pmd.wallRaycastHits);
    public Func<Collider[], int> GetColliders(GameObject gameObject) => GetColliders(gameObject.transform.position, gameObject.transform.forward);
    public Func<Collider[], int> GetColliders(Transform trans) => GetColliders(trans.position, trans.forward);

    public Func<Collider[], int> GetColliders(Vector3 location, Vector3 direction) {
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 pos = location + rotation * centerOffset;

        return checkShape switch {
            CheckShape.Sphere => (Collider[] x) => {
                return Physics.OverlapSphereNonAlloc(pos, size1, x, layers);
            }
            ,
            CheckShape.Cube => (Collider[] x) => {
                return Physics.OverlapBoxNonAlloc(pos, halfExtents, x, rotation, layers);
            }
            ,
            _ => null,
        };
    }
    public static Func<RaycastHit[], int> GetRayCastColliders(Vector3 location, Vector3 direction, LayerMask layerMask) {
        Quaternion rotation = Quaternion.LookRotation(direction);
        Ray ray = new Ray(location, direction);
        return (RaycastHit[] x) => Physics.RaycastNonAlloc(ray, x, 1, layerMask);
    }
    public void Gizmo(GameObject gameObject) => Gizmo(gameObject.transform);
    public void Gizmo(Transform trans) => Gizmo(trans.position, trans.forward);
    public void Gizmo(Vector3 location, Vector3 direction) {
        if (!doDrawGizmo) return;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Gizmos.matrix = Matrix4x4.TRS(location + rotation * centerOffset, rotation, Vector3.one);
        Gizmos.color = Color.red;
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
            DrawProp(ref y, position, property, "doDrawGizmo");
            DrawProp(ref y, position, property, "wireFrame");

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
    Sphere,
    Ray
}