using AbilitySystem;

public interface IHealth {

    void Damage(EntityDamage damage);
    void Heal(EntityDamage heal);
    void Die();
}