using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnemy : Entity
{
    public override void Fall()
    {
        Destroy(gameObject);
    }

    public override void InitComponents()
    {
        throw new System.NotImplementedException();
    }

    public override void SetHigherLayer()
    {
        gameObject.layer = LayerList.EnemyDashing;
    }

    public override void SetNormalLayer()
    {
        gameObject.layer = LayerList.Enemy;
    }
}