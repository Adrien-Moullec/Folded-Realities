using AbilitySystem;

/// <summary>
/// Base information for entities or objects that can take damage.
/// </summary>
public interface IHealth {

    void Damage(EntityDamage damage);
    void Heal(EntityDamage heal);
    void Die();
    void SetMaxHealth();
}