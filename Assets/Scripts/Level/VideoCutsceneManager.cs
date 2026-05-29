using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Video cutscene manager complete with skip button and automatic scene transition after finish.
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(LevelExit))]
public class VideoCutsceneManager : MonoBehaviour {
    [Header("Skip Button Objects")]
    [Tooltip("Skip button reference.")]
    [SerializeField] Button skipButton;
    [Tooltip("Canvas reference for setting alpha value.")]
    [SerializeField] CanvasGroup canvasGroupAlpha;

    [Space]
    [Header("Skip Settings")]
    [Tooltip("Fading time of skip button.")]
    [SerializeField, Min(0.5f)] float skipButtonTime = 5;
    [Tooltip("Alpha value of skip button over time.")]
    [SerializeField] AnimationCurve skipButtonAlphaCurve;


    [Tooltip("Video player reference.")]
    private VideoPlayer videoPlayer;
    [Tooltip("Player input reference.")]
    private PlayerInput playerInput;
    [Tooltip("Current time of skip button appearance.")]
    private float time = 0;
    [Tooltip("Current alpha value of skip button.")]
    private float currentAlpha = 0;
    [Tooltip("Is the skip button active on screen.")]
    private bool isSkipButtonShowing = false;
    [Tooltip("Click action from player, this activates the skip button appearance.")]
    InputAction clickInput;

    /// <summary>
    /// Setup components and buttons, then prepare the video.
    /// </summary>
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

    /// <summary>
    /// Unlock cursor.
    /// </summary>
    void Start() {
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Disable input on disable script.
    /// </summary>
    void OnDisable() {
        clickInput.performed -= input => ButtonCheck();
        videoPlayer.prepareCompleted -= input => videoPlayer.Play();
        videoPlayer.loopPointReached -= input => OnEnd();
    }

    /// <summary>
    /// Check for if the button is displaying to stop button flash on screen re-click
    /// </summary>
    void ButtonCheck() {
        if (isSkipButtonShowing) {
            time = 0;
            currentAlpha = canvasGroupAlpha.alpha;
            return;
        }

        StartCoroutine(SkipButtonControl());
    }

    /// <summary>
    /// Control the alpha value of the skip button.
    /// </summary>
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

    /// <summary>
    /// Prepare the video but wait for the player to load in properly.
    /// </summary>
    IEnumerator PrepareVid() {
        yield return new WaitForSeconds(1);
        videoPlayer.Prepare();
    }

    /// <summary>
    /// Activate next scene on video end.
    /// </summary>
    private void OnEnd() {
        GetComponent<LevelExit>().NextScene();
    }
}
