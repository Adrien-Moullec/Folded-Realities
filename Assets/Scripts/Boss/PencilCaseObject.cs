using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PencilCaseObject : MonoBehaviour {

    Animator animator;
    PencilCase pencilCase;
    float time;
    public void OnSpawn(PencilCase pencilCase) {
        this.pencilCase = pencilCase;
        time = 0;
        animator = GetComponent<Animator>();
        animator.Play("PencilRoll", 0);
    }
}