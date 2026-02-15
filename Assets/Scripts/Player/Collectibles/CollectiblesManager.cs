using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    [Header("Normal Collectibles")]
    public int normalCount = 0;
    public TMP_Text normalCountText;

    [Header("Special Collectibles")]
    public Image[] puzzlePieces; // size = 4
    private int specialCount = 0;

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
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Special"))
        {
            if (specialCount < puzzlePieces.Length)
            {
                puzzlePieces[specialCount].enabled = true;
                specialCount++;
            }

            Destroy(other.gameObject);
        }
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
            puzzlePieces[i].enabled = false;
        }
    }
}
