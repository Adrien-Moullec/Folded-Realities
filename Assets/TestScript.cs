using UnityEngine;

using UnityEditor;

public class TestScript : MonoBehaviour {
    [SerializeField] public AreaColliderCheck areaAffects;

    void OnDrawGizmos() {
        areaAffects.Gizmo(gameObject);
    }
}

[CustomEditor(typeof(TestScript))]
public class TestScriptEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        TestScript AreaAffects = target as TestScript;

        if (GUILayout.Button("GET COLLIDERS")) {
            RaycastHit[] x = new RaycastHit[10];
            AreaAffects.areaAffects.GetColliders(AreaAffects.gameObject).Invoke(x);
            Debug.Log("TEST");
            foreach (var n in x) {
                if (n.collider == null) continue;
                Debug.Log(n.collider.name);
            }
        }
    }
}
