using UnityEngine;
using UnityEngine.UI;
using AbilitySystem;

public class RadialMenuManager : MonoBehaviour {

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

    [Header("Settings")]
    [SerializeField]
    private int segmentCount = 3;

    private float segmentAngle;

    private int currentIndex = 0;

    private bool wheelOpen = false;

    void Start() {

        Debug.Log(
            "RADIAL MENU START"
        );

        segmentAngle =
            360f / segmentCount;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                false
            );

            Debug.Log(
                "RADIAL MENU ROOT DISABLED"
            );
        } else {

            Debug.LogError(
                "RADIAL MENU ROOT IS NULL"
            );
        }

        ResetHighlights();

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

            Debug.Log(
                "M KEY DOWN"
            );

            OpenWheel();
        }

        if (
            Input.GetKeyUp(
                KeyCode.M
            )
        ) {

            Debug.Log(
                "M KEY UP"
            );

            CloseWheel();
        }

        if (
            wheelOpen
        ) {

            UpdateSelection();
        }
    }

    void OpenWheel() {

        Debug.Log(
            "OPENING WHEEL"
        );

        wheelOpen = true;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                true
            );

            Debug.Log(
                "RADIAL MENU ENABLED"
            );
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    void CloseWheel() {

        Debug.Log(
            "CLOSING WHEEL"
        );

        wheelOpen = false;

        if (
            radialMenuRoot != null
        ) {

            radialMenuRoot.SetActive(
                false
            );

            Debug.Log(
                "RADIAL MENU DISABLED"
            );
        }

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        TriggerSelectedForm();

        ResetHighlights();
    }

    void UpdateSelection() {

        Vector2 centerScreenPosition =
            RectTransformUtility
            .WorldToScreenPoint(
                null,
                center.position
            );

        Vector2 mousePos =
            Input.mousePosition;

        Vector2 delta =
            mousePos
            - centerScreenPosition;

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

        if (
            playerAbilityController
            == null
        ) {

            Debug.LogError(
                "PLAYER ABILITY CONTROLLER NOT ASSIGNED"
            );

            return;
        }

        switch (currentIndex) {

            case 0:

                Debug.Log(
                    "TRANSFORMING TO BEARSET"
                );

                playerAbilityController
                    .InputTransitionName(
                        "Bear"
                    );

                break;

            case 1:

                Debug.Log(
                    "TRANSFORMING TO SPIDERSET"
                );

                playerAbilityController
                    .InputTransitionName(
                        "Spider"
                    );

                break;

            case 2:

                Debug.Log(
                    "TRANSFORMING TO FROGSET"
                );

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