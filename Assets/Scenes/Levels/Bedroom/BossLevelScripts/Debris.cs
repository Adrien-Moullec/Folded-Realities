using UnityEngine;

public class Debris : MonoBehaviour {
    public float moveSpeed = 8f;

    public float resetHeight = 60f;

    public float bottomHeight = -60f;

    void Update() {
        transform.position +=
            Vector3.up
            * moveSpeed
            * Time.deltaTime;

        if (
            transform.position.y
            > resetHeight
        ) {
            Vector3 pos =
                transform.position;

            pos.y = bottomHeight;

            transform.position = pos;
        }
    }
}