using UnityEngine;
using System.Collections;

public class BossBounce : MonoBehaviour {

    public float bounceForce = 12f;

    public float flashTime = 0.2f;

    Renderer[] renderers;

    Color[] originalColors;

    void Start() {

        renderers =
            GetComponentsInChildren<Renderer>();

        originalColors =
            new Color[
                renderers.Length
            ];

        for (int i = 0; i < renderers.Length; i++) {

            originalColors[i] =
                renderers[i].material.color;
        }
    }

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            !other.CompareTag("Player")
        ) {
            return;
        }

        CharacterController cc =
            other.GetComponent<CharacterController>();

        if (cc != null) {

            Vector3 bounce =
                Vector3.up *
                bounceForce;

            cc.Move(
                bounce *
                Time.deltaTime
            );
        }

        StartCoroutine(
            FlashRed()
        );
    }

    IEnumerator FlashRed() {

        foreach (
            Renderer r
            in renderers
        ) {

            r.material.color =
                Color.red;
        }

        yield return new WaitForSeconds(
            flashTime
        );

        for (int i = 0; i < renderers.Length; i++) {

            renderers[i].material.color =
                originalColors[i];
        }
    }
}