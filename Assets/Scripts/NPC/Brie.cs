using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Brie : MonoBehaviour {
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }
    void Start() {
        Debug.Log("PLAY CROSSFADE");
        animator.CrossFade("Reception", 0);
        Debug.Log("PLAYED CROSSFADE");
    }

    // Update is called once per frame
    void Update() {

    }
}