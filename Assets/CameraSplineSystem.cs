using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class CameraSplineSystem : MonoBehaviour
{
    [SerializeField] GameObject testObject;
    SplineContainer splineContainer;
    Spline playerPath;
    Spline cameraPath;
    ICamera player = null;
    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        playerPath = splineContainer.Splines[0];
        cameraPath = splineContainer.Splines[1];
    }
    void Update()
    {
        if (!ValidSplines()) return;

        testObject.transform.position = GetCameraPosition(
            splineContainer,
            playerPath,
            cameraPath,
            (float3)PlayerManager.player.transform.position
        );
    }
    public static Vector3 GetCameraPosition(SplineContainer container, Spline playerPath, Spline cameraPath, float3 worldPlayer)
    {
        if (container == null || container.Splines.Count < 2) { 
            Debug.LogWarning("SplineContainer Error");
            return Vector3.zero;
        }

        SplineUtility.GetNearestPoint(playerPath, worldPlayer, out _, out float t);
        
        float3 camPos = playerPath == null
            ? float3.zero
            : cameraPath.EvaluatePosition(t);

        return (Vector3)camPos;
    }

    bool ValidSplines()
    {
        if (splineContainer.Splines.Count >= 2)
            if (splineContainer.Splines[0].Count > 2 && splineContainer.Splines[1].Count > 2)
                return true;
        return false;
    }
    void OnTriggerEnter(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
}