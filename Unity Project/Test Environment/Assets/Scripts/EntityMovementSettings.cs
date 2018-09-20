using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/MovementSettings", fileName = "entityMovementData")]
public class EntityMovementSettings : ScriptableObject
{
    [SerializeField] private float movementSpeed;

    public float MovementSpeed { get { return movementSpeed; } }
}