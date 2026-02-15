using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class CameraSplineSystem : CameraArea
{
    [SerializeField] GameObject playObj;
    [SerializeField] float cameraLerp = 1;

    [Tooltip("Control where the camera transitions between player and spline point. Positive value makes camera transition before the curve, negative value makes camera transition after entering the curve.")]
    [SerializeField] float cameraBoundaryTransition = 0.3f;
    SplineContainer splineContainer;
    Spline playerPath;
    Spline cameraPath;
    ICamera player = null;

    Vector3 camTargetPos;
    Vector3 playerPathPoint;
    float tPoint;

    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        playerPath = splineContainer.Splines[0];
        cameraPath = splineContainer.Splines[1];
    }

    public override Vector3 GetCameraPosition(Camera camera)
    {
        if (!ValidSplines()) return camera.transform.position;

        camTargetPos = GetCameraPosition(
            splineContainer,
            playerPath,
            cameraPath,
            (float3)PlayerManager.player.transform.position,
            out playerPathPoint,
            out tPoint
        );
        Debug.Log(tPoint);
        playObj.transform.position = playerPathPoint;
        return Vector3.MoveTowards(
            camera.transform.position,
            camTargetPos,
            cameraLerp * Time.deltaTime * Vector3.Distance(camTargetPos, camera.transform.position)
        );
    }
    public static Vector3 GetCameraPosition(SplineContainer container, Spline playerPath, Spline cameraPath, float3 worldPlayer, out Vector3 playerPathPos, out float TPoint)
    {
        Debug.Log("Container: "+container+", playerPath: "+(playerPath!=null)+", camPath: "+(cameraPath!=null)+".");
        if (container == null || container.Splines.Count < 2) { 
            Debug.LogWarning("SplineContainer Error");
            playerPathPos = Vector3.zero;
            TPoint = 0;
            return Vector3.zero;
        }

        SplineUtility.GetNearestPoint(playerPath, worldPlayer, out float3 nearest, out TPoint);
        playerPathPos = (Vector3)nearest;
        
        float3 camPos = playerPath == null
            ? float3.zero
            : cameraPath.EvaluatePosition(TPoint);

        return (Vector3)camPos;
    }

    bool ValidSplines()
    {
        if (splineContainer.Splines.Count >= 2)
            if (splineContainer.Splines[0].Count > 2 && splineContainer.Splines[1].Count > 2)
                return true;
        return false;
    }
}