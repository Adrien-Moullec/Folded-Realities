public interface IHealth {
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void Damage(float amount);
    void Heal(float amount);
    void Die();
}
