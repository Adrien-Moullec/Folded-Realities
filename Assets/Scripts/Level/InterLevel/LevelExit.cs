using UnityEngine;

public class LevelExit : MonoBehaviour, IInteractable {
    public TargetLevel targetLevel;
    public string targetSpawnID = "1";
    public bool activatedByWalkingIntoArea = false;

    [HideInInspector] public bool locked;

    bool triggered = false;

    void OnTriggerEnter(Collider other) {
        if (locked || !other.CompareTag("Player") || triggered || !activatedByWalkingIntoArea)
            return;
        NextScene();
    }

    public void OnInteract() {
        if (locked || triggered)
            return;
        NextScene();
    }

    public void OnCancelInteract() {

    }

    void NextScene() {
        triggered = true;
        SpawnData.spawnID = targetSpawnID;
        GameplaySystem.instance.LoadScene(targetLevel, TransitionType.Iris);
    }
}