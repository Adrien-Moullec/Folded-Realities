using UnityEngine;

public class WindZone : MonoBehaviour {
    public float windStrength = 14f;
    public float maxDistance = 10f;
    public float upwardLift = 0.8f;

    public float againstWindMultiplier = 0.4f;
    public float withWindBoost = 1.05f;

    public float windOnTime = 3f;
    public float windOffTime = 5f;

    public ParticleSystem[] windParticles;

    private float timer;
    private bool windActive;

    private void Start() {
        SetWind(false);
    }

    private void Update() {
        timer += Time.deltaTime;

        if (windActive && timer >= windOnTime) {
            SetWind(false);
        } else if (!windActive && timer >= windOffTime) {
            SetWind(true);
        }
    }

    private void SetWind(bool state) {
        windActive = state;
        timer = 0f;

        if (windParticles != null) {
            for (int i = 0; i < windParticles.Length; i++) {
                if (windParticles[i] == null) {
                    continue;
                }

                if (state) {
                    windParticles[i].Clear();
                    windParticles[i].Play(true);
                } else {
                    windParticles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!windActive) {
            return;
        }

        if (!other.CompareTag("Player")) {
            return;
        }

        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller == null) {
            return;
        }

        Vector3 toPlayer = (other.transform.position - transform.position).normalized;

        float alignment = Vector3.Dot(transform.forward, toPlayer);
        if (alignment <= 0) {
            return;
        }

        float distance = Vector3.Distance(transform.position, other.transform.position);
        float falloff = 1 - Mathf.Clamp01(distance / maxDistance);

        Vector3 windForce = transform.forward * windStrength * falloff;

        windForce += Vector3.up * upwardLift * falloff;

        if (!controller.isGrounded) {
            windForce *= 1.5f;
        }

        Vector3 playerInputDir = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        if (playerInputDir.magnitude > 0.1f) {
            playerInputDir.Normalize();

            float moveVsWind = Vector3.Dot(playerInputDir, transform.forward);

            if (moveVsWind < -0.3f) {
                windForce *= 1f + Mathf.Abs(moveVsWind) * 0.5f;
                controller.Move(-playerInputDir * againstWindMultiplier * Time.deltaTime);
            } else if (moveVsWind > 0.3f) {
                windForce *= withWindBoost;
            }
        }

        controller.Move(windForce * Time.deltaTime);
    }
}