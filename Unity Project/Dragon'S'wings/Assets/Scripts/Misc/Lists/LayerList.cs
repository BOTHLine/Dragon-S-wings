using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerList
{
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int PlayerHigher = LayerMask.NameToLayer("PlayerHigher");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int EnemyHigher = LayerMask.NameToLayer("EnemyHigher");
    public static readonly int EntityFallingCheck = LayerMask.NameToLayer("EntityFallingCheck");
    public static readonly int LevelFallingCheck = LayerMask.NameToLayer("LevelFallingCheck");
    public static readonly int LevelEdge = LayerMask.NameToLayer("LevelEdge");
    public static readonly int Obstacle = LayerMask.NameToLayer("Obstacle");
    public static readonly int Hook = LayerMask.NameToLayer("Hook");
}