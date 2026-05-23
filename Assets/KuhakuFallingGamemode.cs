using AbilitySystem;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KuhakuFallingGamemode : MonoBehaviour, IHealth {

    [SerializeField] PlayerHealthCanvas healthCanvas;
    [SerializeField] float speed = 1;
    [SerializeField] float acceleration = 1;
    [SerializeField] int health = 100;

    CharacterController characterController;
    PlayerInput playerInput;
    InputAction moveAction;
    Vector2 Movement;

    float velocity;

    public void Damage(EntityDamage damage) {
        health -= (int)damage.amount;
        healthCanvas?.UpdateHearts(health);
        if (health <= 0) Die();
    }

    public void Die() {
        throw new System.NotImplementedException();
    }

    public void Heal(EntityDamage heal) {
        health -= (int)heal.amount;
        healthCanvas?.UpdateHearts(health);
    }

    public void SetMaxHealth() {
        throw new System.NotImplementedException();
    }

    void OnEnable() {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        moveAction.performed += i => Movement = i.ReadValue<Vector2>();
        moveAction.canceled += i => Movement = Vector2.zero;
        healthCanvas?.UpdateHearts(health);
    }

    void Update() {
        velocity = Mathf.MoveTowards(velocity, Movement.x, acceleration * Time.deltaTime);
        characterController.Move(new Vector3(velocity, 0, 0) * Time.deltaTime * speed);
    }
}
