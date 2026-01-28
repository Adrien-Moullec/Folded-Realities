using UnityEngine;

[CreateAssetMenu(fileName = "Create/Origami/Stats Object", menuName = "Origami Stats")]
public class EntityStatsSO : ScriptableObject
{
    [SerializeField, Min(0)] public float strengh = 5;
    [SerializeField, Min(0)] public float maxSpeed = 2;
    [SerializeField, Min(0)] public float sprintSpeed = 4;
    [SerializeField, Min(0)] public float fallMultiplier = 1;
}
