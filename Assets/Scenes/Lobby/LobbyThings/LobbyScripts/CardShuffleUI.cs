using UnityEngine;
using System.Collections;

public class CardShuffleUI : MonoBehaviour {
    public GameObject cardA;
    public GameObject cardB;

    public float shuffleSpeed = 0.05f;
    public float shuffleDuration = 1.5f;

    private bool isShuffling = false;

    public void StartShuffle() {
        if (!isShuffling)
            StartCoroutine(Shuffle());
    }

    IEnumerator Shuffle() {
        isShuffling = true;

        float timer = 0f;
        bool toggle = false;

        while (timer < shuffleDuration) {
            toggle = !toggle;

            cardA.SetActive(toggle);
            cardB.SetActive(!toggle);

            yield return new WaitForSeconds(shuffleSpeed);
            timer += shuffleSpeed;
        }

        cardA.SetActive(true);
        cardB.SetActive(true);

        isShuffling = false;
    }
}