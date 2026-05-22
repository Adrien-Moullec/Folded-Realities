using UnityEngine;

public class GameplaySystems : MonoBehaviour {

    static GameplaySystems instance;

    void Awake() {

        if (instance != null) {

            Destroy(gameObject);

            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}