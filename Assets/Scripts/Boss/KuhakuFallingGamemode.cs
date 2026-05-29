using System.Collections;

using AbilitySystem;

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player script specifically for Kuhaku's falling gamemode.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class KuhakuFallingGamemode : MonoBehaviour, IHealth {

    [Tooltip("Player health canvas reference.")]
    [SerializeField] PlayerHealthCanvas healthCanvas;
    [Tooltip("Material reference for setting flashing toon effect on hit.")]
    [SerializeField] Material shader;
    [Tooltip("Horizontal speed of Kuhaku.")]
    [SerializeField] float speed = 1;
    [Tooltip("Horizontal acceleration of Kuhaku.")]
    [SerializeField] float acceleration = 1;
    [Tooltip("Health of Kuhaku.")]
    [SerializeField] int health = 100;

    [Tooltip("Character controller reference.")]
    CharacterController characterController;
    [Tooltip("Player Input reference.")]
    PlayerInput playerInput;
    [Tooltip("Movement action from input system.")]
    InputAction moveAction;
    [Tooltip("Movement value from input system.")]
    Vector2 Movement;

    [Tooltip("Velocity of player at frame time.")]
    float velocity;
    [Tooltip("Invincibility status of Kuhaku.")]
    bool invincible = false;
    [Tooltip("Invincibility time for Kuhaku.")]
    float invincibilityTime = 1;

    /// <summary>
    /// Damage done to Kuhaku
    /// </summary>
    public void Damage(EntityDamage damage) {
        if (invincible) return;

        health -= (int)damage.amount;
        healthCanvas?.UpdateHearts(health);

        if (health <= 0) Die();
        else StartCoroutine(InvincibilityFrames());
    }

    /// <summary>
    /// Respawn on death
    /// </summary>
    public void Die() {
        GameplaySystem.instance.Respawn();
    }

    /// <summary>
    /// Heal if item picked up.
    /// </summary>
    public void Heal(EntityDamage heal) {
        health -= (int)heal.amount;
        healthCanvas?.UpdateHearts(health);
    }

    /// Not possible to set max health
    public void SetMaxHealth() {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Setup player movement inputs.
    /// </summary>
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

    /// <summary>
    /// Update velocity based on input.
    /// </summary>
    void Update() {
        velocity = Mathf.MoveTowards(velocity, Movement.x, acceleration * Time.deltaTime);
        characterController.Move(new Vector3(velocity, 0, 0) * Time.deltaTime * speed);
    }
}
