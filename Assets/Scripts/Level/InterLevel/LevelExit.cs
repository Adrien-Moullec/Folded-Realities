using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour,
    IInteractable {

    public string nextSceneName =
        "Bedroom";

    public string targetSpawnID =
        "1";

    [HideInInspector]
    public bool locked;

    bool triggered = false;

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            locked
            ||
            !other.CompareTag(
                "Player"
            )
            ||
            triggered
        ) {
            return;
        }

        NextScene();
    }

    public void OnInteract() {

        if (locked) {
            return;
        }

        NextScene();
    }

    public void OnCancelInteract() {
    }

    void NextScene() {

        triggered = true;

        Debug.Log(
            gameObject.name +
            " SETTING SpawnID TO: "
            + targetSpawnID
        );

        SpawnData.spawnID =
            targetSpawnID;

        SceneTransition.Instance
            .TransitionToScene(
                nextSceneName
            );
    }
}