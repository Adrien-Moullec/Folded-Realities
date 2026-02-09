using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform center;
    public RectTransform selectObject;
    public GameObject radialMenuRoot;

    [Header("Radial Settings")]
    [SerializeField] private int segmentCount = 6;
    [SerializeField] private float spriteRotationOffset = -120f;

    private float segmentAngle;
    private bool isRadialMenuActive = false;

    private int currentIndex = -1;

    void Start()
    {
        segmentAngle = 360f / segmentCount;
        radialMenuRoot.SetActive(false);
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
            angle += 360f;

        angle += segmentAngle / 2f;

        if (angle >= 360f)
            angle -= 360f;

        currentIndex = Mathf.FloorToInt(angle / segmentAngle);

        float finalRotation = (currentIndex * segmentAngle) + spriteRotationOffset;

        selectObject.localRotation = Quaternion.Euler(0f, 0f, finalRotation);
    }

    private void OnSegmentClicked(int index)
    {
        Debug.Log("Clicked segment: " + index);

       
        radialMenuRoot.SetActive(false);
        isRadialMenuActive = false;

        
    }
}
