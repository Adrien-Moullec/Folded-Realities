using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;

public class SceneTransition : MonoBehaviour {
    public Image irisImage;
    Material irisMaterial;
    public float speed = 1.5f;
    public float respawnSpeed = 0.7f;
    // Prevents overlapping transitions
    bool transitioning = false;

    void Start() {
        // Gets iris material reference
        irisMaterial = irisImage.material;
        // Starts transition fully open
        irisMaterial.SetFloat("_Radius", 1f);
    }

    public void TransitionToScene(string targetLevel, int spawnLocId) {
        // Prevents duplicate transitions
        if (transitioning)
            return;
        StartCoroutine(TransitionRoutine(targetLevel, spawnLocId));
    }

    public IEnumerator RespawnTransition(bool savePoint = true) {

        if (transitioning)
            yield break;

        transitioning = true;
        irisMaterial.SetFloat("_Radius", 0f);
        yield return null;
        // Respawns player at checkpoint
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.RespawnPlayer();

        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);

        float radius = 0f;

        while (radius < 1f) {
            radius += Time.deltaTime * respawnSpeed;
            irisMaterial.SetFloat("_Radius", radius);
            yield return null;
        }

        transitioning = false;
    }
    IEnumerator TransitionRoutine(string sceneName, int spawnLocId) {

        transitioning = true;
        float radius = 1f;
        while (radius > 0f) {
            radius -= Time.deltaTime * speed;
            irisMaterial.SetFloat("_Radius", radius);
            yield return null;
        }

        radius = 0f;
        irisMaterial.SetFloat("_Radius", radius);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();

        irisMaterial = irisImage.material;

        yield return new WaitForSeconds(0.8f);
        if (sceneName == GameplayScenes.MainMenu.ToString())
            GameplaySystem.slot = -1;

        radius = 0f;
        // Respawns player at correct spawn location
        irisMaterial.SetFloat("_Radius", radius);
        CheckpointManager.Instance?.RespawnPlayerIntoLevel(spawnLocId);

        while (radius < 1f) {
            radius += Time.deltaTime * (speed * 0.7f);
            irisMaterial.SetFloat("_Radius", radius);
            yield return null;
        }

        // fully open
        irisMaterial.SetFloat("_Radius", 1f);
        transitioning = false;
    }
    public IEnumerator BossDeathTransition() {

        if (transitioning)
            yield break;
        // Closes iris during boss death
        transitioning = true;
        float radius = 1f;
        while (radius > 0f) {
            radius -= Time.deltaTime * speed;
            irisMaterial.SetFloat("_Radius", radius);
            yield return null;
        }

        // Holds black screen after transition
        yield return new WaitForSeconds(1f);
    }
}