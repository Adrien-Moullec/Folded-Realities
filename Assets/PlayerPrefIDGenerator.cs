using NUnit.Framework;

using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerPrefIDGenerator : MonoBehaviour {
    public static int GetIdGeneration(GameObject gameObject, string scene) {
        return Animator.StringToHash(
            gameObject.name +
            gameObject.transform.position.ToString() +
            scene.ToString()
        );
    }
    /*
        public int IdGenerator => GetIdGeneration(gameObject, SceneManager.GetActiveScene().name.ToString());
        [SerializeField] public bool isDebug = false;
        Vector3 startPos;
        void Awake() {
            startPos = transform.position;
            if (!GameplaySystem.IsIdActive(IdGenerator)) {
                Debug.Log(gameObject.name + " IdInactive");
                gameObject.SetActive(false);
            } else {
                if (isDebug)
                    Debug.Log(gameObject.name + "Active");
            }
        }
        public void SetPlayerPrefIdActive(bool active) {
            GameplaySystem.SetIdActive(IdGenerator, active);
        }*/
}