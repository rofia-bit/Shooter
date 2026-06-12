using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchState : MovementBaseState
{
    float timeEntered;

    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Crouching", true);
        timeEntered = Time.time;
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Time.time - timeEntered < 0.2f) return;

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
            else ExitState(movement, movement.Walk);
        }

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.crouchBackSpeed;
        else movement.currentMoveSpeed = movement.crouchSpeed;
    }

    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}