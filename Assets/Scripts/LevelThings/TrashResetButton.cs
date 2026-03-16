using UnityEngine;

public class TrashResetButton : MonoBehaviour {

    [SerializeField] Rigidbody trashRB;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject resetPlatform;
    [SerializeField] GameObject stuckPrompt;
    [SerializeField] TrashTriggerDrop dropTrigger;

    [SerializeField] GameObject overHerePrompt;

    void OnTriggerEnter(Collider other) {

        if (!other.CompareTag("Player")) {
            return;
        }

        Debug.Log("Resetting trash");

        trashRB.linearVelocity = Vector3.zero;
        trashRB.angularVelocity = Vector3.zero;

        trashRB.transform.position = spawnPoint.position;

        resetPlatform.SetActive(false);
        stuckPrompt.SetActive(false);

        if (overHerePrompt != null) {
            overHerePrompt.SetActive(true);
        }

        dropTrigger.ResetTrigger();
    }
}