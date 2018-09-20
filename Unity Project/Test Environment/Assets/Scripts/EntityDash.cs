using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDash
{
    private readonly EntityInput entityInput;
    private readonly Rigidbody2D rigidbody2DToMove;
    private readonly EntityDashSettings entityDashSettings;

    private bool canDash = true;

    public event System.Action OnDashStart = delegate { };
    public event System.Action OnDashEnd = delegate { };

    public EntityDash(EntityInput entityInput, Rigidbody2D rigidbody2DToMove, EntityDashSettings entityDashSettings)
    {
        this.entityInput = entityInput as PlayerInput;
        this.rigidbody2DToMove = rigidbody2DToMove;
        this.entityDashSettings = entityDashSettings;

        entityInput.OnDashInput += Dash;
    }

    public void Dash(Vector2 dashTargetPosition)
    {
        if (!canDash)
            return;

        canDash = false;
        OnDashStart();

        // Dash itself
        rigidbody2DToMove.velocity = (dashTargetPosition - (Vector2)rigidbody2DToMove.transform.position).normalized * entityDashSettings.DashSpeed;
        rigidbody2DToMove.MovePosition();

        canDash = true;
        OnDashEnd();
    }
}