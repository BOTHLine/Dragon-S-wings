using UnityEngine;

public class PlayerInput : EntityInput
{
    public override void ReadInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        base.RaiseOnMoveInput(new Vector2(horizontal, vertical).normalized);

        if (Input.GetButtonDown("Dash"))
            base.RaiseOnDashInputEvent(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}