using UnityEngine;

using Unity.Cinemachine;

/// <summary>
/// Cinemachine base interactability class for setting virtual camera priority.
/// </summary>
public abstract class CinemachineOrigami : MonoBehaviour {
    [Tooltip("Cinemachine reference to adjust priority.")]
    [SerializeField] protected CinemachineCamera vcamToActivate;
    [Tooltip("Activation high priority of camera to set.")]
    [SerializeField] protected int areaPriority = 10;
    [Tooltip("Default priority of a cinemachine.")]
    [SerializeField] protected int defaultPriority = 0;

    /// <summary>
    /// Set virtual camera to inactive by default.
    /// </summary>
    void Awake() {
        vcamToActivate.gameObject.SetActive(false);
    }
    /// <summary>
    /// Automatically set all cinemachines to target Kuhaku.
    /// </summary>
    void Start() {

        vcamToActivate.Target.TrackingTarget = PlayerManager.player.transform;
        vcamToActivate.Target.LookAtTarget = PlayerManager.player.transform;
    }

    /// <summary>
    /// Set camera's priority to highest.
    /// </summary>
    public virtual void SetCameraHighPriority() {
        vcamToActivate.gameObject.SetActive(true);
        if (vcamToActivate == null) return;
        vcamToActivate.Priority = areaPriority;
    }
    /// <summary>
    /// Set camera's priority to lowest.
    /// </summary>
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
