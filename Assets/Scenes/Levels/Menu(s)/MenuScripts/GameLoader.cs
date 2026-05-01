using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

    public static int slotToLoad = -1;

    public void LoadSlot(int slot) {

        if (PlayerPrefs.GetInt("Slot" + slot + "_Exists", 0) != 1) {
            Debug.Log("No save in this slot");
            return;
        }

        slotToLoad = slot;

        string sceneName = PlayerPrefs.GetString("Slot" + slot + "_Scene", "");

        if (sceneName != "") {
            SceneManager.LoadScene(sceneName);
        }
    }
}