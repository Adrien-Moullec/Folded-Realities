using Unity.Mathematics;

using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class PlayerPathCameraRailSpline : CameraArea {
    [SerializeField] GameObject playObj;
    [SerializeField] float cameraLerp = 1;

    [Tooltip("Distance to spline when camera starts lerp to destination.")]
    [SerializeField, Min(0)] float cameraBoundaryTransitionStart = 5f;
    [Tooltip("Distance to spline when camera ends lerp to destination.")]
    [SerializeField, Min(0)] float cameraBoundaryTransitionEnd = 5f;
    SplineContainer splineContainer;
    Spline playerPath;
    Spline cameraPath;

    Vector3 camTargetPos;
    Vector3 playerPathPoint;
    float splineDistance;
    float tPoint;
    float deltaLerp;

    void Awake() {
        splineContainer = GetComponent<SplineContainer>();
        playerPath = splineContainer.Splines[0];
        cameraPath = splineContainer.Splines[1];
    }

    public override Vector3 GetCameraPosition(Camera camera, Vector3 cameraHolderPos) {
        if (!ValidSplines()) return camera.transform.position;

        camTargetPos = GetCameraPosition(
            splineContainer,
            playerPath,
            cameraPath,
            (float3)PlayerManager.player.transform.position,
            out playerPathPoint,
            out tPoint,
            out splineDistance
        );

        deltaLerp = Mathf.InverseLerp(cameraBoundaryTransitionEnd, cameraBoundaryTransitionStart + cameraBoundaryTransitionEnd, splineDistance);

        playObj.transform.position = playerPathPoint;
        return Vector3.MoveTowards(
            camera.transform.position,
            Vector3.Lerp(camTargetPos, cameraHolderPos, deltaLerp),
            cameraLerp * Time.deltaTime * Vector3.Distance(camTargetPos, camera.transform.position)
        );
    }
    public static Vector3 GetCameraPosition(SplineContainer container, Spline playerPath, Spline cameraPath, float3 worldPlayer, out Vector3 playerPathPos, out float TPoint, out float splineDistance) {
        if (container == null || container.Splines.Count < 2) {
            Debug.LogWarning("SplineContainer Error");
            playerPathPos = Vector3.zero;
            TPoint = 0;
            splineDistance = 0;
            return Vector3.zero;
        }

        SplineUtility.GetNearestPoint(playerPath, worldPlayer, out float3 nearest, out TPoint);
        splineDistance = Vector3.Distance(nearest, worldPlayer);
        playerPathPos = (Vector3)nearest;

        float3 camPos = playerPath == null
            ? float3.zero
            : cameraPath.EvaluatePosition(TPoint);


        return (Vector3)camPos;
    }

    bool ValidSplines() {
        if (splineContainer.Splines.Count >= 2)
            if (splineContainer.Splines[0].Count > 2 && splineContainer.Splines[1].Count > 2)
                return true;
        return false;
    }
}