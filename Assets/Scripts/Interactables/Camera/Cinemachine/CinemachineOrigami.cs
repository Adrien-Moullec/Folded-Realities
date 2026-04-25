using UnityEngine;

using Unity.Cinemachine;

public abstract class CinemachineOrigami : MonoBehaviour {
    [SerializeField] protected CinemachineCamera vcamToActivate;
    [SerializeField] protected int areaPriority = 10;
    [SerializeField] protected int defaultPriority = 0;

    void Awake() {
        vcamToActivate.gameObject.SetActive(false);
    }

    public virtual void SetCameraHighPriority() {
        vcamToActivate.gameObject.SetActive(true);
        if (vcamToActivate == null) return;
        vcamToActivate.Priority = areaPriority;
    }
    public virtual void SetCameraDefaultPriority() {
        vcamToActivate.gameObject.SetActive(false);
        if (vcamToActivate == null) return;
        vcamToActivate.Priority = defaultPriority;
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
