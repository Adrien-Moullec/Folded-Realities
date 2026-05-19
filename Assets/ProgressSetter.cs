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

        int currentProgress =
            PlayerPrefs.GetInt(
                "Progress",
                0
            );

        if (
            setProgressTo >
            currentProgress
        ) {

            PlayerPrefs.SetInt(
                "Progress",
                setProgressTo
            );

            PlayerPrefs.Save();

            Debug.Log(
                "PROGRESS UPDATED TO: "
                + setProgressTo
            );
        } else {

            Debug.Log(
                "Progress remains at: "
                + currentProgress
            );
        }
    }
}