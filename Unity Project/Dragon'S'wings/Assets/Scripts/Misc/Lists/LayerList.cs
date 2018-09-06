using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerList
{
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int PlayerFalling = LayerMask.NameToLayer("PlayerFalling");
    public static readonly int PlayerDashing = LayerMask.NameToLayer("PlayerDashing");
    public static readonly int LevelTrigger = LayerMask.NameToLayer("LevelTrigger");
    public static readonly int Obstacle = LayerMask.NameToLayer("Obstacle");
    public static readonly int Hook = LayerMask.NameToLayer("Hook");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int EnemyDashing = LayerMask.NameToLayer("EnemyDashing");
    public static readonly int EnemyFalling = LayerMask.NameToLayer("EnemyFalling");
    public static readonly int PlayerFallingCheck = LayerMask.NameToLayer("PlayerFallingCheck");
    public static readonly int LevelEdge = LayerMask.NameToLayer("LevelEdge");
}