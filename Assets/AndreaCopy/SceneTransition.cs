using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {

    public static SceneTransition Instance;

    public Image irisImage;

    Material irisMaterial;

    public float speed = 1.5f;

    public float respawnSpeed = 0.7f;

    bool transitioning = false;

    void Awake() {

        if (
            Instance == null
        ) {

            Instance = this;

            DontDestroyOnLoad(
                gameObject
            );

        } else {

            Destroy(gameObject);
        }
    }

    void Start() {

        irisMaterial =
            irisImage.material;

        irisMaterial.SetFloat(
            "_Radius",
            1f
        );
    }

    void Update() {

        if (
            Input.GetKeyDown(
                KeyCode.T
            )
        ) {

            StartCoroutine(
                TransitionRoutine(
                    "Bedroom"
                )
            );
        }
    }

    public void TransitionToScene(
        string sceneName
    ) {

        if (
            transitioning
        ) {
            return;
        }

        StartCoroutine(
            TransitionRoutine(
                sceneName
            )
        );
    }

    public IEnumerator RespawnTransition(
        GameObject player
    ) {

        if (transitioning) {
            yield break;
        }

        transitioning = true;

        irisMaterial.SetFloat(
            "_Radius",
            0f
        );

        yield return null;

        if (
            CheckpointManager.Instance
            != null
        ) {

            CheckpointManager.Instance
                .RespawnPlayer(
                    player
                );
        }

        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(
            0.5f
        );

        float radius = 0f;

        while (radius < 1f) {

            radius +=
                Time.deltaTime
                * respawnSpeed;

            irisMaterial.SetFloat(
                "_Radius",
                radius
            );

            yield return null;
        }

        transitioning = false;
    }
    IEnumerator TransitionRoutine(
        string sceneName
    ) {

        transitioning = true;

        float radius = 1f;

        while (radius > 0f) {

            radius -=
                Time.deltaTime
                * speed;

            irisMaterial.SetFloat(
                "_Radius",
                radius
            );

            yield return null;
        }


        radius = 0f;

        irisMaterial.SetFloat(
            "_Radius",
            radius
        );

        yield return new WaitForSeconds(
            0.5f
        );


        SceneManager.LoadScene(
            sceneName
        );

        yield return null;
        yield return null;
        yield return null;

        yield return new WaitForEndOfFrame();


        irisMaterial =
            irisImage.material;


        yield return new WaitForSeconds(
            0.8f
        );

        radius = 0f;

        irisMaterial.SetFloat(
            "_Radius",
            radius
        );

        while (radius < 1f) {

            radius +=
                Time.deltaTime
                * (speed * 0.7f);

            irisMaterial.SetFloat(
                "_Radius",
                radius
            );

            yield return null;
        }

        // fully open
        irisMaterial.SetFloat(
            "_Radius",
            1f
        );

        transitioning = false;
    }
    public IEnumerator BossDeathTransition() {

        if (transitioning) {
            yield break;
        }

        transitioning = true;

        float radius = 1f;

        while (radius > 0f) {

            radius -=
                Time.deltaTime
                * speed;

            irisMaterial.SetFloat(
                "_Radius",
                radius
            );

            yield return null;
        }

        // hold black
        yield return new WaitForSeconds(
            1f
        );
    }
}