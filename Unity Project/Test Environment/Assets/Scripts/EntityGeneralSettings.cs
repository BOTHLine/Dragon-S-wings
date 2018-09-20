using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/GeneralSettings", fileName = "entityGeneralData")]
public class EntityGeneralSettings : ScriptableObject
{
    [SerializeField] private bool isPlayer;

    [SerializeField] private EntityMovementSettings entityMovementSettings;
    [SerializeField] private EntityDashSettings entityDashSettings;
    
    public bool IsPlayer { get { return isPlayer; } }

    public EntityMovementSettings EntityMovementSettings { get { return entityMovementSettings; } }
    public EntityDashSettings EntityDashSettings { get { return entityDashSettings; } }
}