using System.Collections;

using AbilitySystem;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KuhakuFallingGamemode : MonoBehaviour, IHealth {

    [SerializeField] PlayerHealthCanvas healthCanvas;
    [SerializeField] Material shader;
    [SerializeField] float speed = 1;
    [SerializeField] float acceleration = 1;
    [SerializeField] int health = 100;

    CharacterController characterController;
    PlayerInput playerInput;
    InputAction moveAction;
    Vector2 Movement;

    float velocity;
    bool invincible = false;
    float invincibilityTime = 1;

    public void Damage(EntityDamage damage) {
        if (invincible) return;

        health -= (int)damage.amount;
        healthCanvas?.UpdateHearts(health);

        if (health <= 0) Die();
        else StartCoroutine(InvincibilityFrames());
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
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        moveAction.performed += i => Movement = i.ReadValue<Vector2>();
        moveAction.canceled += i => Movement = Vector2.zero;
        healthCanvas?.UpdateHearts(health);
    }
    // Andrea's function
    public IEnumerator InvincibilityFrames() {
        // Enables invulnerability state
        invincible = true;
        float time = 0;
        while (time < invincibilityTime) {
            time += Time.deltaTime;
            // Creates flashing sine wave effect
            float f = Mathf.Abs(Mathf.Sin(time * 8 / invincibilityTime));
            Debug.Log(f);
            shader.SetFloat("_DamageFlash01", f);
            yield return null;
        }
        // Resets shader flash effect
        shader.SetFloat("_DamageFlash01", 0);
        // Disables invulnerability state
        invincible = false;
    }


    void Update() {
        velocity = Mathf.MoveTowards(velocity, Movement.x, acceleration * Time.deltaTime);
        characterController.Move(new Vector3(velocity, 0, 0) * Time.deltaTime * speed);
    }
}
