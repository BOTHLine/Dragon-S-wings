using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/DashSettings", fileName = "entityDashData")]
public class EntityDashSettings : ScriptableObject
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashMaxRange;
    [SerializeField] private float dashCooldown;

    public float DashSpeed { get { return dashSpeed; } }
    public float DashMaxRange { get { return dashMaxRange; } }
    public float DashCoolDown { get { return DashCoolDown; } }
}