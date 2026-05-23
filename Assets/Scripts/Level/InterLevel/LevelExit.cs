using UnityEngine;

public class LevelExit : MonoBehaviour,
    IInteractable {

    public string nextSceneName =
        "Bedroom";

    public string targetSpawnID =
        "1";

    [HideInInspector]
    public bool locked;

    bool triggered = false;

    void OnTriggerEnter(Collider other) {
        if (locked || !other.CompareTag("Player") || triggered)
            return;
        NextScene();
    }

    public void OnInteract() {
        if (locked)
            return;
        NextScene();
    }

    public void OnCancelInteract() {
    }

    void NextScene() {

        triggered = true;
        SpawnData.spawnID = targetSpawnID;
        GameplaySystem.instance.LoadScene(GameplayScenes.Bedroom, TransitionType.Iris);
    }
}