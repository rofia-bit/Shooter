using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Idle", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.dir.magnitude > 0.1f) ExitState(movement, movement.Walk);
        else if (Keyboard.current.cKey.wasPressedThisFrame) ExitState(movement, movement.Crouch);
    }

    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Idle", false);
        movement.SwitchState(state);
    }
}