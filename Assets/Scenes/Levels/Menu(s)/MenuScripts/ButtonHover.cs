using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Vector3 originalScale;

    void Start() {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale = originalScale * 1.1f;

        if (UIAudio.Instance != null) {
            UIAudio.Instance.PlayHover();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale = originalScale;
    }
}