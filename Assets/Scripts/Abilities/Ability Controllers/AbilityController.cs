using Unity.Mathematics;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class AbilityController : MonoBehaviour, IMovement, IAbilityCooldown
    {
        [Header("Body Components")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] public EntityBody entityBody;

        protected virtual void Awake()
        {
            entityBody.iMovement = this;
            entityBody.iAbilityCooldown = this;
        }

        public abstract void IMoveEntity(Vector3 direction);

        protected virtual void OnDrawGizmos()
        {
            if (entityBody.feet == null || !drawGizmos) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(entityBody.feet.transform.position + entityBody.feet.center, entityBody.feet.radius);
        }

        public abstract void IRotateEntity(Vector3 movement);
        public virtual void OnAbilityUsed(AbilityData data)
        {
            if (data.isRecharging) return;
        }
    }
}