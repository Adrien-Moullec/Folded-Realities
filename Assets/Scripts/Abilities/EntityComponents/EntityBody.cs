using System;
using UnityEngine;

[Serializable]
public class EntityBody
{
    public GameObject body;
    public SphereCollider feet;
    [HideInInspector] public CharacterController controller;
}