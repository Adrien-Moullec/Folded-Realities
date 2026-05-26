using NUnit.Framework;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefIDGenerator : MonoBehaviour {
    public int IdGenerator => GetIdGeneration(gameObject, SceneManager.GetActiveScene().name.ToString());
    [SerializeField] public bool isDebug = false;
    Vector3 startPos;
    public static int GetIdGeneration(GameObject gameObject, string scene) {
        return Animator.StringToHash(
            gameObject.name +
            gameObject.transform.position.ToString() +
            scene.ToString()
        );
    }
    void Awake() {
        startPos = transform.position;
        if (!GameplaySystem.IsIdActive(IdGenerator)) {
            if (isDebug) Debug.Log(gameObject.name + " IdInactive");
            gameObject.SetActive(false);
        } else {
            if (isDebug)
                Debug.Log(gameObject.name + "Active");
        }
    }
    public void SetPlayerPrefIdActive(bool active) {
        GameplaySystem.SetIdActive(IdGenerator, active);
    }
}