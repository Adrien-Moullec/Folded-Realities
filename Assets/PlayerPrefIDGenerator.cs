using NUnit.Framework;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefIDGenerator : MonoBehaviour {
    public int IdGenerator => GetIdGeneration(gameObject, SceneManager.GetActiveScene().name.ToString());
    public static int GetIdGeneration(GameObject gameObject, string scene) {
        return Animator.StringToHash(
            gameObject.name +
            gameObject.transform.position.ToString() +
            scene.ToString()
        );
    }
    [SerializeField] public bool isDebug = false;
    Vector3 startPos;
    void Awake() {
        startPos = transform.position;
        if (!GameplaySystem.IsIdActive(IdGenerator)) {
            Debug.Log("IdInactive");
            gameObject.SetActive(false);
        } else {
            if (isDebug) Debug.Log(GameplaySystem.IsIdActive(IdGenerator));
        }
    }
    public void SetPlayerPrefIdActive(bool active) {
        GameplaySystem.SetIdActive(IdGenerator, active);
    }
}
