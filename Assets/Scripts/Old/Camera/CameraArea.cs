/*using UnityEngine;

public abstract class CameraArea : MonoBehaviour
{
    ICamera player = null;
    public abstract Vector3 GetCameraPosition(Camera camera, Vector3 cameraHolderPos);
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
}*/