using UnityEngine;

[RequireComponent(typeof(PlayerAbilityController))]
public class BaseEnemyController : MonoBehaviour
{
    [Space]
    [Header("Script Managers")]
    [SerializeField] PlayerAbilityController AbilityController;
    void Awake()
    {
        
    }
}
