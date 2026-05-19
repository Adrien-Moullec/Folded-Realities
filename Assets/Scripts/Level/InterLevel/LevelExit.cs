using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour,
    IInteractable {

    public string nextSceneName =
        "Bedroom";

    public string targetSpawnID =
        "1";

    bool triggered =
        false;

    void OnTriggerEnter(
        Collider other
    ) {

        if (
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

        NextScene();
    }

    public void OnCancelInteract() {
    }

    void NextScene() {

        triggered = true;

        Debug.Log(
            "SETTING SpawnID: "
            + targetSpawnID
        );

        PlayerPrefs.SetInt(
            "SpawnDoorID",
            int.Parse(
                targetSpawnID
            )
        );

        PlayerPrefs.Save();

        SceneTransition.Instance
            .TransitionToScene(
                nextSceneName
            );
    }
}