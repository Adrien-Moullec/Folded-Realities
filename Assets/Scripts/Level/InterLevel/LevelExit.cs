using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour, IInteractable {
    // Target level data for scene transition
    public TargetLevel targetLevel;
    public bool activatedByWalkingIntoArea = false;
    public Vector3 spawnPos;
    [SerializeField] float gizmos = 1;


    public bool locked;
    // Returns final calculated spawn position
    [HideInInspector] public Vector3 SpawnPos => transform.position + spawnPos;
    // Prevents duplicate scene transitions
    bool triggered = false;
    // Prevents activation if locked or already triggered
    void OnTriggerEnter(Collider other) {
        if (locked || !other.CompareTag("Player") || triggered || !activatedByWalkingIntoArea)
            return;
        NextScene();
        GameplaySystem.SetSceneSavePoint(SceneManager.GetActiveScene().name, transform.position + spawnPos);
    }
    // Prevents manual interaction while locked
    public void OnInteract() {
        if (locked || triggered)
            return;
        GameplaySystem.SetSceneSavePoint(SceneManager.GetActiveScene().name, SpawnPos);
        NextScene();
    }

    public void OnCancelInteract() {
        // Empty interface method
    }

    public void NextScene() {
        triggered = true;
        Debug.Log(targetLevel.targetScene.ToString());
        // Loads next scene with iris transition
        GameplaySystem.instance.LoadScene(targetLevel, TransitionType.Iris);
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position + spawnPos, gizmos);
    }
}