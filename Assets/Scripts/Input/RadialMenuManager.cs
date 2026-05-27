using UnityEngine;
using UnityEngine.UI;

using AbilitySystem;

public class RadialMenuManager : MonoBehaviour {

    // UI center point used for angle calculations
    [Header("UI References")]
    public RectTransform center;

    public GameObject radialMenuRoot;

    [Header("Segment Images")]
    public Image bearSegment;

    public Image spiderSegment;

    public Image frogSegment;

    [Header("Normal Colours")]
    public Color normalColor = Color.white;

    [Header("Highlight Colour")]
    public Color highlightColor = Color.yellow;

    [Header("Player")]
    public PlayerAbilityController playerAbilityController;
    // Total radial menu segments
    [Header("Settings")]
    [SerializeField]
    private int segmentCount = 3;
    // Angle size for each segment
    private float segmentAngle;

    private int currentIndex = 0;

    private bool wheelOpen = false;

    void Start() {
        // Calculates angle size for segments
        segmentAngle =
            360f / segmentCount;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                false
            );

        }

        ResetHighlights();
        // Locks and hides cursor during gameplay
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void Update() {

        if (
            Input.GetKeyDown(
                KeyCode.M
            )
        ) {
            OpenWheel();
        }

        if (
            Input.GetKeyUp(
                KeyCode.M
            )
        ) {

            CloseWheel();
        }
        // Updates selection while menu is open
        if (
            wheelOpen
        ) {

            UpdateSelection();
        }
    }

    void OpenWheel() {

        wheelOpen = true;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                true
            );
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    void CloseWheel() {

        wheelOpen = false;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                false
            );
        }

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        TriggerSelectedForm();

        ResetHighlights();
    }
    // Gets radial menu center position
    void UpdateSelection() {

        Vector2 centerScreenPosition =
            RectTransformUtility
            .WorldToScreenPoint(
                null,
                center.position
            );
        // Gets mouse position
        Vector2 mousePos =
            Input.mousePosition;
        // Calculates mouse offset from center
        Vector2 delta =
            mousePos
            - centerScreenPosition;
        // Converts mouse direction into angle
        float angle =
            Mathf.Atan2(
                delta.y,
                delta.x
            )
            * Mathf.Rad2Deg;

        if (angle < 0f) {

            angle += 360f;
        }

        angle +=
            segmentAngle / 2f;

        if (angle >= 360f) {

            angle -= 360f;
        }
        // Determines active segment
        currentIndex =
            Mathf.FloorToInt(
                angle / segmentAngle
            );

        HighlightCurrentSegment();
    }

    void HighlightCurrentSegment() {

        ResetHighlights();

        switch (currentIndex) {

            case 0:

                if (
                    bearSegment != null
                ) {

                    bearSegment.color =
                        highlightColor;
                }

                break;

            case 1:

                if (
                    spiderSegment != null
                ) {

                    spiderSegment.color =
                        highlightColor;
                }

                break;

            case 2:

                if (
                    frogSegment != null
                ) {

                    frogSegment.color =
                        highlightColor;
                }

                break;
        }
    }

    void ResetHighlights() {

        if (
            bearSegment != null
        ) {

            bearSegment.color =
                normalColor;
        }

        if (
            spiderSegment != null
        ) {

            spiderSegment.color =
                normalColor;
        }

        if (
            frogSegment != null
        ) {

            frogSegment.color =
                normalColor;
        }
    }

    void TriggerSelectedForm() {
        // Prevents errors if controller missing
        if (
            playerAbilityController
            == null
        ) {

            return;
        }

        switch (currentIndex) {

            case 0:

                playerAbilityController
                    .InputTransitionName(
                        "Bear"
                    );

                break;

            case 1:

                playerAbilityController
                    .InputTransitionName(
                        "Spider"
                    );

                break;

            case 2:

                playerAbilityController
                    .InputTransitionName(
                        "Frog"
                    );

                break;
        }
    }

    public void SetWheelActive(
        bool active
    ) {

    }

    public string OnSegmentClicked() {

        return "";
    }
}