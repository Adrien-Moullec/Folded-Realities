using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public int checkpointIndex;

    public Renderer checkpointRenderer;

    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    private bool activated = false;

    private Material checkpointMaterial;

    void Start() {
        if (checkpointRenderer != null) {
            // Create unique material instance
            checkpointMaterial = checkpointRenderer.material;

            SetColor(inactiveColor);
        }
    }

    void SetColor(Color c) {
        if (checkpointMaterial == null) {
            return;
        }

        if (checkpointMaterial.HasProperty("_BaseColor")) {
            checkpointMaterial.SetColor("_BaseColor", c);
        }

        if (checkpointMaterial.HasProperty("_Color")) {
            checkpointMaterial.SetColor("_Color", c);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        if (activated) {
            return;
        }

        CheckpointManager.Instance.SetCheckpoint(transform.position + Vector3.up * 1f, checkpointIndex);

        ActivateCheckpoint();
    }

    void ActivateCheckpoint() {
        activated = true;

        SetColor(activeColor);

        Debug.Log("Checkpoint " + checkpointIndex + " activated");
    }
}