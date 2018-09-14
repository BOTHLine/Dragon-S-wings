using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnemy : Entity
{
    public override void Fall()
    {
        Destroy(gameObject);
    }

    public override Vector2 GetAimingVector()
    {
        throw new System.NotImplementedException();
    }

    public override void InitOtherComponents()
    {
        throw new System.NotImplementedException();
    }

    public override void InitOwnComponents()
    {
        throw new System.NotImplementedException();
    }

    public override void SetHigherLayer()
    {
        gameObject.layer = LayerList.EnemyHigher;
    }

    public override void SetNormalLayer()
    {
        gameObject.layer = LayerList.Enemy;
    }
}