using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

    public static int slotToLoad = -1;

    public void LoadSlot(int slot) {
        slotToLoad = slot;
        SceneManager.LoadScene("GameScene");
    }
}