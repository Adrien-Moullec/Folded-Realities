using System.Collections;

using UnityEngine;

using UnityEditor;

public class RadialMenuManager : MonoBehaviour {
    [Header("UI References")]
    public RectTransform center;
    public RectTransform selectObject;
    public GameObject radialMenuRoot;

    /*[Header("Forms")]
    public GameObject kuhaku_jump;
    public GameObject kuhaku_krane;

    [Header("Animation")]
    public Animation playerAnimation;

    public float transformTime = 0.4f;
    public float scrunchTime = 0.9f;*/

    [Header("Radial Settings")]
    [SerializeField] private int segmentCount = 6;
    [SerializeField] private float spriteRotationOffset = -120f;

    private float segmentAngle;
    private int currentIndex = 0;

    //private GameObject currentModel;

    void Start() {
        segmentAngle = 360f / segmentCount;
        radialMenuRoot.SetActive(false);

        //currentModel = kuhaku_jump;

        //kuhaku_jump.SetActive(true);
        //kuhaku_krane.SetActive(false);
    }

    public void SetWheelActive(bool active) => radialMenuRoot.SetActive(active);

    void Update() {

        // Toggle radial menu with E
        if (Input.GetKeyDown(KeyCode.E)) {
            radialMenuRoot.SetActive(!radialMenuRoot.activeSelf);
        }

        if (radialMenuRoot.activeSelf) {
            UpdateSelection();
        }
    }

    private void UpdateSelection() {
        Vector2 centerScreenPosition = RectTransformUtility.WorldToScreenPoint(null, center.position);

        Vector2 mousePos = Input.mousePosition;
        Vector2 delta = mousePos - centerScreenPosition;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle < 0f) {
            angle += 360f;
        }

        angle += segmentAngle / 2f;

        if (angle >= 360f) {
            angle -= 360f;
        }

        currentIndex = Mathf.FloorToInt(angle / segmentAngle);

        float finalRotation = (currentIndex * segmentAngle) + spriteRotationOffset;

        selectObject.localRotation = Quaternion.Euler(0f, 0f, finalRotation);
    }

    public string OnSegmentClicked() {


        /*
        if (isSwitching) {
            return;
        }*/

        switch (currentIndex) {
            case 0: return "Kuhaku";
            case 1: return "Crane";
            default: Debug.LogError("Not a sufficient index"); return "";
        }

        /*radialMenuRoot.SetActive(false);
        isRadialMenuActive = false;*/
    }

    /*
    public void StartPlayerToCrane() => StartCoroutine(PlayerToCrane());
    IEnumerator PlayerToCrane() {
        isSwitching = true;

        Vector3 lockedPos = kuhaku_jump.transform.position;
        Quaternion lockedRot = kuhaku_jump.transform.rotation;

        playerAnimation.Play("Transform");

        float timer = 0f;

        while (timer < transformTime) {
            kuhaku_jump.transform.SetPositionAndRotation(lockedPos, lockedRot);
            timer += Time.deltaTime;
            yield return null;
        }

        playerAnimation.Play("Scrunch");

        timer = 0f;

        while (timer < scrunchTime) {
            kuhaku_jump.transform.SetPositionAndRotation(lockedPos, lockedRot);
            timer += Time.deltaTime;
            yield return null;
        }

        kuhaku_jump.SetActive(false);

        kuhaku_krane.SetActive(true);
        kuhaku_krane.transform.SetPositionAndRotation(lockedPos, lockedRot);

        currentModel = kuhaku_krane;

        isSwitching = false;
    }

    public void StartCraneToPlayer() => StartCoroutine(CraneToPlayer());
    IEnumerator CraneToPlayer() {
        Vector3 pos = kuhaku_krane.transform.position;
        Quaternion rot = kuhaku_krane.transform.rotation;

        kuhaku_krane.SetActive(false);

        kuhaku_jump.SetActive(true);
        kuhaku_jump.transform.SetPositionAndRotation(pos, rot);

        currentModel = kuhaku_jump;

        yield return null;
    }
    */
}

/*
[CustomEditor(typeof(RadialMenuManager))]
public class RadialMenuManagerEditor : Editor {
    SerializedProperty lookAtPoint;


    public override void OnInspectorGUI() {
        RadialMenuManager rmm = (RadialMenuManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Player to Crane")) {
            rmm.StartPlayerToCrane();
        }
        if (GUILayout.Button("Crane to Player")) {
            rmm.StartCraneToPlayer();
        }
    }
}*/