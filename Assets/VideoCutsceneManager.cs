using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(PlayerInput))]
public class VideoCutsceneManager : MonoBehaviour {
    private VideoPlayer videoPlayer;
    private PlayerInput playerInput;
    [Header("Skip Button Objects")]
    [SerializeField] Button skipButton;
    [SerializeField] CanvasGroup canvasGroupAlpha;

    [Space]
    [Header("Skip Settings")]
    [SerializeField, Min(0.5f)] float skipButtonTime = 5;
    [SerializeField] AnimationCurve skipButtonAlphaCurve;
    private float time = 0;
    private float currentAlpha = 0;
    private bool isSkipButtonShowing = false;
    InputAction clickInput;

    void OnEnable() {
        skipButton.gameObject.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        playerInput = GetComponent<PlayerInput>();
        clickInput = playerInput.actions["Click"];
        clickInput.performed += input => ButtonCheck();
        videoPlayer.prepareCompleted += input => videoPlayer.Play();
        videoPlayer.loopPointReached += input => OnEnd();
        skipButton.onClick.AddListener(() => OnEnd());
        StartCoroutine(PrepareVid());
    }

    void OnDisable() {
        clickInput.performed -= input => ButtonCheck();
        videoPlayer.prepareCompleted -= input => videoPlayer.Play();
        videoPlayer.loopPointReached -= input => OnEnd();
    }

    void ButtonCheck() {
        if (isSkipButtonShowing) {
            time = 0;
            currentAlpha = canvasGroupAlpha.alpha;
            return;
        }

        StartCoroutine(SkipButtonControl());
    }

    IEnumerator SkipButtonControl() {
        isSkipButtonShowing = true;
        skipButton.gameObject.SetActive(true);
        time = 0;
        currentAlpha = 0;
        while (time < skipButtonTime) {
            currentAlpha = Mathf.Clamp01(currentAlpha + Time.deltaTime);
            time += Time.deltaTime;
            canvasGroupAlpha.alpha = Mathf.Min(skipButtonAlphaCurve.Evaluate(time / skipButtonTime), currentAlpha);
            yield return null;
        }
        isSkipButtonShowing = false;
        skipButton.gameObject.SetActive(false);
    }

    IEnumerator PrepareVid() {
        yield return new WaitForSeconds(1);
        videoPlayer.Prepare();
    }

    private void OnEnd() {
        GameplaySystem.instance.LoadScene(GameplayScenes.Tutorial2, TransitionType.Iris);
    }
}
