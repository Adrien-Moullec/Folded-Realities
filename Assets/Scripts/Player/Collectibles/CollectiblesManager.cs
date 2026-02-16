using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CollectibleManager : MonoBehaviour
{
    [Header("Normal Collectibles")]
    public int normalCount = 0;
    public TMP_Text normalCountText;

    [Header("Special Collectibles")]
    public Image[] puzzlePieces;
    private int specialCount = 0;

    [Header("Idle Float Settings")]
    public float hoverHeight = 0.25f;
    public float hoverSpeed = 2f;
    public float idleRotationSpeed = 90f;

    [Header("Pickup Effect Settings")]
    public float pickupFloatSpeed = 2f;
    public float pickupRotationSpeed = 360f;
    public float destroyDelay = 0.6f;

    private void Start()
    {
        UpdateNormalUI();
        ResetPuzzleUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            normalCount++;
            UpdateNormalUI();
            StartCoroutine(PlayPickupEffect(other.gameObject));
        }

        if (other.CompareTag("Special"))
        {
            if (specialCount < puzzlePieces.Length)
            {
                puzzlePieces[specialCount].enabled = true;
                specialCount++;
            }

            StartCoroutine(PlayPickupEffect(other.gameObject));
        }
    }

    private IEnumerator PlayPickupEffect(GameObject obj)
    {
        // Disable collider
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Stop idle floating script
        CollectibleIdle idle = obj.GetComponent<CollectibleIdle>();
        if (idle != null)
        {
            idle.enabled = false;
        }

        float timer = 0f;

        while (timer < destroyDelay)
        {
            obj.transform.position += Vector3.up * pickupFloatSpeed * Time.deltaTime;
            obj.transform.Rotate(Vector3.up * pickupRotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }

    private void UpdateNormalUI()
    {
        if (normalCountText != null)
        {
            normalCountText.text = normalCount.ToString();
        }
    }

    private void ResetPuzzleUI()
    {
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            if (puzzlePieces[i] != null)
            {
                puzzlePieces[i].enabled = false;
            }
        }
    }
}
