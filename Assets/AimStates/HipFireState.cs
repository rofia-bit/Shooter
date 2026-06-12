using UnityEngine;
using UnityEngine.InputSystem;

public class HipFireState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.anim.SetBool("Aiming", false);
        aim.isAiming = false;
    }

    public override void UpdateState(AimStateManager aim)
    {
        if (Mouse.current.leftButton.isPressed)
            aim.SwitchState(aim.Aim);
    }
}