using System.Collections;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform center;
    public RectTransform selectObject;
    public GameObject radialMenuRoot;

    [Header("Model References")]
    public GameObject Crane;
    public GameObject XBot;

    private GameObject currentModel;

    [Header("Radial Settings")]
    [SerializeField] private int segmentCount = 6;
    [SerializeField] private float spriteRotationOffset = -120f;

    [Header("Spin Timing")]
    [SerializeField] private float animationStartOffset = 0.2f;
    [SerializeField] private float animationEndCutoff = 0.15f;

    private float segmentAngle;
    private bool isRadialMenuActive = false;
    private bool isSwitching = false;

    private int currentIndex = -1;

    void Start()
    {
        segmentAngle = 360f / segmentCount;
        radialMenuRoot.SetActive(false);

        currentModel = XBot;

        XBot.SetActive(true);
        Crane.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRadialMenuActive = !isRadialMenuActive;
            radialMenuRoot.SetActive(isRadialMenuActive);
        }

        if (isRadialMenuActive)
        {
            UpdateSelection();

            if (Input.GetMouseButtonDown(0))
            {
                OnSegmentClicked(currentIndex);
            }
        }
    }

    private void UpdateSelection()
    {
        Vector2 centerScreenPosition =
            RectTransformUtility.WorldToScreenPoint(null, center.position);

        Vector2 mousePos = Input.mousePosition;
        Vector2 delta = mousePos - centerScreenPosition;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle < 0f)
        {
            angle += 360f;
        }

        angle += segmentAngle / 2f;

        if (angle >= 360f)
        {
            angle -= 360f;
        }

        currentIndex = Mathf.FloorToInt(angle / segmentAngle);

        float finalRotation = (currentIndex * segmentAngle) + spriteRotationOffset;

        selectObject.localRotation = Quaternion.Euler(0f, 0f, finalRotation);
    }

    private void OnSegmentClicked(int index)
    {
        if (isSwitching)
        {
            return;
        }

        if (index == 0 && currentModel != XBot)
        {
            StartCoroutine(SwitchModels(XBot));
        }

        if (index == 1 && currentModel != Crane)
        {
            StartCoroutine(SwitchModels(Crane));
        }

        radialMenuRoot.SetActive(false);
        isRadialMenuActive = false;
    }

    IEnumerator SwitchModels(GameObject newModel)
    {
        isSwitching = true;

        float swapPoint = 0.5f;

        float spinPercent = 0f;

        // --- START CURRENT SPIN ---
        if (currentModel == XBot)
        {
            Animation anim = currentModel.GetComponent<Animation>();
            if (anim != null && anim.clip != null)
            {
                anim.Play(anim.clip.name);
                yield return new WaitForSeconds(anim.clip.length * swapPoint);

                spinPercent = swapPoint;
            }
        }
        else if (currentModel == Crane)
        {
            CraneFloat crane = currentModel.GetComponent<CraneFloat>();
            if (crane != null)
            {
                crane.StartSpinFrom(0f);
                yield return new WaitForSeconds(crane.spinDuration * swapPoint);

                spinPercent = swapPoint;
            }
        }

        currentModel.SetActive(false);

        newModel.SetActive(true);
        currentModel = newModel;

        // --- CONTINUE SPIN ON NEW ---
        if (currentModel == XBot)
        {
            Animation anim = currentModel.GetComponent<Animation>();
            if (anim != null && anim.clip != null)
            {
                anim.Play(anim.clip.name);
                anim[anim.clip.name].time = anim.clip.length * spinPercent;
            }
        }
        else if (currentModel == Crane)
        {
            CraneFloat crane = currentModel.GetComponent<CraneFloat>();
            if (crane != null)
            {
                crane.StartSpinFrom(spinPercent);
            }
        }

        isSwitching = false;
    }
}