using UnityEngine;

public class ProgressSetter : MonoBehaviour {

    public int setProgressTo;

    bool triggered;

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            triggered
            ||
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        triggered = true;

        PlayerPrefs.SetInt(
            "Progress",
            setProgressTo
        );

        PlayerPrefs.Save();

        Debug.Log(
            "PROGRESS SET TO: "
            + setProgressTo
        );
    }
}