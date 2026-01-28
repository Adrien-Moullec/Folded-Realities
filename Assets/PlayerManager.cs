using UnityEngine;

public class PlayerManager : MonoBehaviour, ICamera
{
    [Space]
    [Header("Camera Settings")]
    [SerializeField] GameplayCamera gameplayCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField, Min(0.01f)] float lerpSpeed = 0.01f;
    private Vector3 GetCameraPosition
    {
        get => camArea!=null ? camArea.cameraLocation + camArea.transform.position : cameraHolder.position;
    }
    CameraArea camArea;
    float deltaCameraLerp = 1;

    void Update()
    {
        CameraSettings();
    }

    void CameraSettings()
    {
        gameplayCamera.transform.position = Vector3.MoveTowards(
            gameplayCamera.transform.position, 
            GetCameraPosition, 
            lerpSpeed * Time.deltaTime
        );
        gameplayCamera.transform.forward = Vector3.MoveTowards(
            gameplayCamera.transform.forward,
            transform.position - GetCameraPosition, 
            lerpSpeed * Time.deltaTime
        );
    }

    public void OnCameraAreaEnter(CameraArea cameraArea)
    {
        camArea = cameraArea;
    }

    public void OnCameraAreaExit()
    {
        camArea = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(GetCameraPosition, Vector3.one);
    }
}
