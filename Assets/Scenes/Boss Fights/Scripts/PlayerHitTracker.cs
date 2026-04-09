using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitTracker : MonoBehaviour {
    public int maxHits = 5;
    private int currentHits = 0;

    public void RegisterHit() {
        currentHits++;

        Debug.Log("Hit! " + currentHits + "/" + maxHits);

        if (currentHits >= maxHits) {
            RestartScene();
        }
    }

    void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}