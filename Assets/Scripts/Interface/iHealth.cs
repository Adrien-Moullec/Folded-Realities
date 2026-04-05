using AbilitySystem;

public interface IHealth {
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void Damage(EntityDamage damage);
    void Heal(EntityDamage heal);
    void Die();
}
