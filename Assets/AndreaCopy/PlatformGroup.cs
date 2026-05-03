using UnityEngine;

[System.Serializable]
public class PlatformGroup {

    public int keyID;
    public GameObject[] platforms;

    public void Show() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].SetActive(true);
        }
    }

    public void Hide() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].SetActive(false);
        }
    }
}