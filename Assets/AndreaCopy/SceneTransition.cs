using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {

    public static SceneTransition Instance;

    public Image irisImage;

    Material irisMaterial;

    public float speed = 1.5f;

    bool transitioning = false;

    void Awake() {

        Instance = this;

        DontDestroyOnLoad(
            gameObject
        );
    }

    void Start() {

        irisMaterial =
            irisImage.material;

        irisMaterial.SetFloat(
            "_Radius",
            1f
        );

        Debug.Log(
            "IRIS READY"
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

        yield return new WaitForSeconds(
            0.1f
        );
        SceneManager.LoadScene(
            sceneName
        );

        yield return null;

        irisMaterial =
            irisImage.material;

        yield return new WaitForSeconds(
            0.1f
        );

        radius = 0f;

        irisMaterial.SetFloat(
            "_Radius",
            radius
        );

        while (radius < 1f) {

            radius +=
                Time.deltaTime
                * speed;

            irisMaterial.SetFloat(
                "_Radius",
                radius
            );

            yield return null;
        }

        transitioning = false;
    }
}