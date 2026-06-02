using UnityEngine;

public class KeyCollectible : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {

        if (!other.CompareTag("Player")) {
            return;
        }

        KeyManager.Instance.CollectKey();

        Destroy(gameObject);
    }
}