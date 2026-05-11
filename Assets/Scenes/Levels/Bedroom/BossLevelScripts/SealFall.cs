using UnityEngine;

public class SealFall : MonoBehaviour {
    public Transform player;

    public float distanceBelow = 15f;

    public float followSpeed = 4f;

    public float horizontalDrift = 2f;

    public float driftSpeed = 2f;

    void Update() {
        Vector3 targetPos =
            player.position;

        targetPos.y -= distanceBelow;

        targetPos.x +=
            Mathf.Sin(
                Time.time
                * driftSpeed
            ) * horizontalDrift;

        transform.position =
            Vector3.Lerp(
                transform.position,
                targetPos,
                followSpeed
                * Time.deltaTime
            );
    }
}