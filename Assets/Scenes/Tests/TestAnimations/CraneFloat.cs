using UnityEngine;

public class CraneFloat : MonoBehaviour
{
    [Header("References")]
    public Transform meshToSpin;

    [Header("Spin")]
    public Vector3 spinAxis = Vector3.forward;
    public float spinDuration = 0.6f;

    [Header("Floating")]
    public float floatHeight = 0.15f;
    public float floatSpeed = 1.5f;

    private Vector3 startPosition;
    private Quaternion meshOriginalRotation;

    private bool isSpinning = false;
    private float spinTimer = 0f;

    void OnEnable()
    {
        startPosition = transform.position;

        if (meshToSpin != null)
        {
            meshOriginalRotation = meshToSpin.localRotation;
        }

        spinTimer = 0f;
        isSpinning = false;
    }

    public void StartSpinFrom(float startPercent)
    {
        spinTimer = spinDuration * startPercent;
        isSpinning = true;
    }

    public float GetSpinPercent()
    {
        return spinTimer / spinDuration;
    }

    void Update()
    {
        if (isSpinning && meshToSpin != null)
        {
            spinTimer += Time.deltaTime;

            float percent = spinTimer / spinDuration;
            float angle = percent * 360f;

            meshToSpin.localRotation =
                meshOriginalRotation *
                Quaternion.AngleAxis(angle, spinAxis);

            if (spinTimer >= spinDuration)
            {
                meshToSpin.localRotation = meshOriginalRotation;
                isSpinning = false;
            }

            return;
        }

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}