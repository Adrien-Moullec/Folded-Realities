using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockLevel : MonoBehaviour {
    public string unlockKey;

    private bool triggered = false;

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || triggered) return;

        triggered = true;

        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();

        Debug.Log(unlockKey + " unlocked!");
        SceneManager.LoadScene(unlockKey);
    }
}