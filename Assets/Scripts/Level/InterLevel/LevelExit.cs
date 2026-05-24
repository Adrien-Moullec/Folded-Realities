using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour, IInteractable {
    public TargetLevel targetLevel;
    public bool activatedByWalkingIntoArea = false;
    public Vector3 spawnPos;
    [SerializeField] float gizmos = 1;


    public bool locked;
    [HideInInspector] public Vector3 SpawnPos => transform.position + spawnPos;

    bool triggered = false;

    void OnTriggerEnter(Collider other) {
        if (locked || !other.CompareTag("Player") || triggered || !activatedByWalkingIntoArea)
            return;
        NextScene();
        GameplaySystem.SetSceneSavePoint(SceneManager.GetActiveScene().name, transform.position + spawnPos);
    }

    public void OnInteract() {
        if (locked || triggered)
            return;
        GameplaySystem.SetSceneSavePoint(SceneManager.GetActiveScene().name, SpawnPos);
        NextScene();
    }

    public void OnCancelInteract() {

    }

    void NextScene() {
        triggered = true;
        GameplaySystem.instance.LoadScene(targetLevel, TransitionType.Iris);
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position + spawnPos, gizmos);
    }
}