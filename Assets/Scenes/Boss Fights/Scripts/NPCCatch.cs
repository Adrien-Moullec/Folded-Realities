using UnityEngine;

public class NPCCatch : MonoBehaviour {
    public Transform player;
    public float catchDistance = 2f;
    public float groundHeight = 1.5f;

    private bool caught = false;

    void Update() {
        if (caught || player == null) return;

        // Only near ground
        if (player.position.y <= groundHeight) {
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist < catchDistance) {
                caught = true;

                transform.SetParent(player);
                transform.localPosition = new Vector3(0.5f, 1f, 0.5f);
            }
        }
    }
}