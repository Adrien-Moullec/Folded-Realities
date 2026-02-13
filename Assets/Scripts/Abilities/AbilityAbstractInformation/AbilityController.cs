using Unity.Mathematics;
using UnityEngine;

public abstract class AbilityController : MonoBehaviour, IMovement
{    
    [Header("Body Components")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] public EntityBody entityBody;

    public abstract void Setup();
    protected virtual void Awake() {
        entityBody.iMovement = this;
        Setup();
    }

    public abstract void IMoveEntity(Vector3 direction);

    protected virtual void OnDrawGizmos()
    {
        if (entityBody.feet == null || !drawGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(entityBody.feet.transform.position + entityBody.feet.center, entityBody.feet.radius);
    }

    public abstract void IRotateEntity(Vector3 movement);
}