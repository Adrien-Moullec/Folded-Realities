using System.Collections;

using UnityEngine;

public class EnemyAttackCycle : MonoBehaviour {
    [SerializeField] GameObject hitbox;

    [SerializeField] float attackInterval = 2f;

    [SerializeField] float hitboxActiveTime = 0.5f;

    void Start() {
        hitbox.SetActive(false);

        StartCoroutine(
            AttackLoop()
        );
    }

    IEnumerator AttackLoop() {
        while (true) {
            yield return new WaitForSeconds(
                attackInterval
            );

            Debug.Log(
                "Hitbox ENABLED"
            );

            hitbox.SetActive(true);

            yield return new WaitForSeconds(
                hitboxActiveTime
            );

            Debug.Log(
                "Hitbox DISABLED"
            );

            hitbox.SetActive(false);
        }
    }
}