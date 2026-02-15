using UnityEngine;

public class CameraArea : MonoBehaviour
{
    [Header("Camera Area Settings")]
    [Space]
    [SerializeField] public Vector3 cameraLocation;

    ICamera player = null;
    void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out player);
        player.OnCameraAreaEnter(this);
    }
    void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out player);
        player.OnCameraAreaExit();
    }
}