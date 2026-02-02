using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform center;
    public RectTransform selectObject;
    public GameObject radialMenuRoot;

    [Header("Radial Settings")]
    [SerializeField]
    private int segmentCount = 6;

    [SerializeField]
    private float spriteRotationOffset = -120f;

    private float segmentAngle;
    private bool isRadialMenuActive = false;

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

            if (isRadialMenuActive)
            {
                radialMenuRoot.SetActive(true);
            }
            else
            {
                radialMenuRoot.SetActive(false);
            }
        }

        if (isRadialMenuActive)
        {
            UpdateSelection();
        }
    }

    private void UpdateSelection()
    {
        Vector2 centerScreenPosition =
            RectTransformUtility.WorldToScreenPoint(null, center.position);

        Vector2 mousePos = (Vector2)Input.mousePosition;
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

        int index = Mathf.FloorToInt(angle / segmentAngle);

        float finalRotation =
            (index * segmentAngle) + spriteRotationOffset;

        selectObject.localRotation =
            Quaternion.Euler(0f, 0f, finalRotation);
    }
}
