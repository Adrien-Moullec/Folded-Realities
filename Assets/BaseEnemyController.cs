using UnityEngine;

[RequireComponent(typeof(AbilityController))]
public class BaseEnemyController : MonoBehaviour
{
    [Space]
    [Header("Script Managers")]
    [SerializeField] AbilityController AbilityController;
    void Awake()
    {
        
    }
}
