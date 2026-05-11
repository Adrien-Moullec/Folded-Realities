using UnityEngine;

public class ScissorsEnemy : MonoBehaviour {
    public float rotateSpeed = 300f;

    public float hoverSpeed = 2f;

    Vector3 startPos;

    bool activeEnemy = false;

    void Start() {
        startPos = transform.position;

        gameObject.SetActive(false);
    }

    void Update() {
        if (!activeEnemy) {
            return;
        }

        transform.Rotate(
            Vector3.forward
            * rotateSpeed
            * Time.deltaTime
        );

        transform.position =
            startPos
            + Vector3.up
            * Mathf.Sin(
                Time.time * hoverSpeed
            );
    }

    public void ActivateScissors() {
        gameObject.SetActive(true);

        activeEnemy = true;
    }
}