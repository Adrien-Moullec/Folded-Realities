using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour {
    public static BossFightManager Instance;

    [Header("Player")]
    public GameObject player;

    public GameObject playerVisuals;

    public CharacterController playerController;

    [Header("NPC")]
    public GameObject npc;

    [Header("Crane")]
    public GameObject craneForm;

    public Transform glideTarget;

    public float glideSpeed = 12f;

    [Header("Falling Section")]
    public GameObject debrisSpawner;

    [Header("Boss Arena")]
    public GameObject bossArena;

    bool sequenceStarted = false;

    void Awake() {
        Instance = this;
    }

    public void TriggerCraneSequence() {
        Debug.Log(
            "TRIGGER CRANE SEQUENCE"
        );

        if (sequenceStarted) {
            Debug.Log(
                "SEQUENCE ALREADY STARTED"
            );

            return;
        }

        StartCoroutine(
            CraneSequenceRoutine()
        );
    }

    IEnumerator CraneSequenceRoutine() {
        Debug.Log(
            "STARTING CRANE ROUTINE"
        );

        sequenceStarted = true;

        if (debrisSpawner != null) {
            Debug.Log(
                "DISABLING DEBRIS SPAWNER"
            );

            debrisSpawner.SetActive(
                false
            );
        }

        FallingKuhaku fk =
            player.GetComponent<
                FallingKuhaku
            >();

        if (fk != null) {
            Debug.Log(
                "SETTING TRANSITION STATE"
            );

            fk.SetTransitionState(
                true
            );
        }

        Rigidbody rb =
            player.GetComponent<
                Rigidbody
            >();

        if (rb != null) {
            Debug.Log(
                "RESETTING PLAYER VELOCITY"
            );

            rb.linearVelocity =
                Vector3.zero;
        }

        Debug.Log(
            "DISABLING PLAYER CONTROLLER"
        );

        playerController.enabled =
            false;

        if (playerVisuals != null) {
            Debug.Log(
                "HIDING PLAYER VISUALS"
            );

            playerVisuals.SetActive(
                false
            );
        } else {
            Debug.LogError(
                "PLAYER VISUALS NOT ASSIGNED"
            );
        }

        if (npc != null) {
            Debug.Log(
                "HIDING NPC"
            );

            npc.SetActive(false);
        }

        if (craneForm == null) {
            Debug.LogError(
                "CRANE FORM MISSING"
            );

            yield break;
        }

        if (glideTarget == null) {
            Debug.LogError(
                "GLIDE TARGET MISSING"
            );

            yield break;
        }

        Debug.Log(
            "SHOWING CRANE"
        );

        craneForm.SetActive(true);

        craneForm.transform.position =
            player.transform.position;

        Debug.Log(
            "CRANE START POSITION: "
            + craneForm.transform.position
        );

        Debug.Log(
            "GLIDE TARGET POSITION: "
            + glideTarget.position
        );

        while (
            Vector3.Distance(
                craneForm.transform.position,
                glideTarget.position
            ) > 0.3f
        ) {
            Vector3 dir =
                (
                    glideTarget.position
                    - craneForm.transform.position
                ).normalized;

            craneForm.transform.position +=
                dir
                * glideSpeed
                * Time.deltaTime;

            Debug.Log(
                "CRANE POS: "
                + craneForm.transform.position
            );

            yield return null;
        }

        Debug.Log(
            "CRANE ARRIVED"
        );

        craneForm.SetActive(false);

        player.transform.position =
            glideTarget.position;

        if (playerVisuals != null) {
            Debug.Log(
                "SHOWING PLAYER VISUALS"
            );

            playerVisuals.SetActive(
                true
            );
        }

        Debug.Log(
            "REENABLING CONTROLLER"
        );

        playerController.enabled =
            true;

        if (fk != null) {
            fk.SetTransitionState(
                false
            );
        }

    }
}