using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour {
    void Start() {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound() {
        if (UIAudio.Instance != null) {
            UIAudio.Instance.PlayClick();
        }
    }
}