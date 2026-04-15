using UnityEngine;

using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEditor;

public class CinemachineArea : MonoBehaviour {
    [SerializeField] private CinemachineCamera vcamToActivate;
    [SerializeField] private int areaPriority = 10;
    [SerializeField] private int defaultPriority = 0;

    [Space]
    [TextArea]
    public string CinemachineBreakdown = "Check this script for notes about how to use cinemachine components.";


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            vcamToActivate.Priority = areaPriority;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            vcamToActivate.Priority = defaultPriority;
        }
    }
}
/// Cinemachine Brain:
/// 
// DAMPING - will stop camera jittering. normally use 0.1 -> 1
// Change default camera blend in CinemachineBrain. Find additional Blend settings in Resources -> CinemachineBlendSettings
// Name the cinemachine camera object to unlock more options in CinemachineBlendSettings

/// FreeLook:
/// Cinemachine Camera - base
/// Cinemachine Orbital Follow - set camera circle range
/// Cinemachine Rotation Composer - Rotate towards target
/// Cinemachine FreeLook Modifier - Additional freelook options
/// Cinemachine Input Axis Controller - Control looking around settings




/*
[CustomEditor(typeof(CinemachineArea))]
[CanEditMultipleObjects]
public class CinemachineAreaEditor : Editor {
    private CinemachineBlenderSettings cinemachineBlenderSettings;
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        cinemachineBlenderSettings = Resources.Load("CinemachineBlendSettings") as CinemachineBlenderSettings;
        EditorGUILayout.ObjectField(cinemachineBlenderSettings);

        //var editor = CreateEditor(cinemachineBlenderSettings);
        //editor.DrawDefaultInspector();
    }
}*/
