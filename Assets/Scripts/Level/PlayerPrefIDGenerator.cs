using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefIDGenerator : MonoBehaviour {

    public int IdGenerator =>
        GetIdGeneration(
            gameObject,
            SceneManager.GetActiveScene().name
        );

    public static int GetIdGeneration(
        GameObject gameObject,
        string scene
    ) {

        return Animator.StringToHash(
            gameObject.name +
            gameObject.transform.position.ToString() +
            scene
        );
    }

    void Awake() {

        if (
            !GameplaySystem.IsIdActive(
                IdGenerator
            )
        ) {

            gameObject.SetActive(
                false
            );
        }
    }

    public void SetCollected() {

        GameplaySystem.SetIdActive(
            IdGenerator,
            false
        );

        GameplaySystem.SaveSettings();
    }
}