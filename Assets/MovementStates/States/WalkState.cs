using UnityEngine;
using UnityEngine.InputSystem;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Keyboard.current.leftShiftKey.isPressed) ExitState(movement, movement.Run);
        else if (Keyboard.current.cKey.wasPressedThisFrame) ExitState(movement, movement.Crouch);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.walkBackSpeed;
        else movement.currentMoveSpeed = movement.walkSpeed;
    }

    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}