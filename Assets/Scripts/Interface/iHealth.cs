using AbilitySystem;

public interface IHealth {
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void Damage(float amount, EntityBody otherBody = null);
    void Heal(float amount, EntityBody otherBody = null);
    void Die();
}
