using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
    public int checkpointIndex;

    public Renderer checkpointRenderer;

    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    private bool activated = false;

    private Material checkpointMaterial;

    void Start() {
        if (checkpointRenderer != null) {
            checkpointMaterial = checkpointRenderer.material;

            SetColor(inactiveColor);
        }
    }
    // Prevents missing material errors
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
    // Only activates for player
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }
        Debug.Log("Given player health checkpoint!");
        // Saves current scene checkpoint position
        GameplaySystem.SetSceneSavePoint(SceneManager.GetActiveScene().name, transform.position);
        if (activated) {
            return;
        }

        if (other.TryGetComponent(out IHealth ihealth))
            ihealth.SetMaxHealth();

        CheckpointManager.Instance.SetCheckpoint(transform.position + Vector3.up * 1f, checkpointIndex);

        ActivateCheckpoint();
    }
    // Marks checkpoint as activated
    void ActivateCheckpoint() {
        activated = true;
        // Updates checkpoint colour
        SetColor(activeColor);
    }
}