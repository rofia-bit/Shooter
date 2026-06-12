using UnityEngine;
using UnityEngine.InputSystem;

public class AimState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.anim.SetBool("Aiming", true);
        aim.isAiming = true;
    }

    public override void UpdateState(AimStateManager aim)
    {
        if (!Mouse.current.leftButton.isPressed)
            aim.SwitchState(aim.HipFire);
    }
}