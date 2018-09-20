using UnityEngine;

public class EntityMovement
{
    private readonly Rigidbody2D rigidbody2DToMove;
    private readonly EntityMovementSettings entityMovementSettings;

    public EntityMovement(EntityInput entityInput, Rigidbody2D rigidbody2DToMove, EntityMovementSettings entityMovementSettings)
    {
        this.rigidbody2DToMove = rigidbody2DToMove;
        this.entityMovementSettings = entityMovementSettings;

        entityInput.OnMoveInput += Move;
    }

    public void Move(Vector2 moveDirection)
    {
        rigidbody2DToMove.velocity = moveDirection * entityMovementSettings.MovementSpeed;
    }
}