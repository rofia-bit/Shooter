using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{
    #region Movement
    public float currentMoveSpeed;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    CharacterController controller;
    #endregion

    [Header("Gravity")]
    [SerializeField] float gravityValue = -9.81f;

    [Header("Smoothing")]
    [SerializeField] float inputSmoothTime = 8f;

    [Header("Ground Check")]
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;



    Vector3 velocity;
    Vector3 spherePos;
    public Animator anim;
    float smoothedHz, smoothedV;

    MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        currentState.UpdateState(this);

        anim.SetFloat("vInput", smoothedV);
        anim.SetFloat("hzInput", smoothedHz);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void GetDirectionAndMove()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        hzInput = (keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0);
        vInput = (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0);

        smoothedHz = Mathf.Lerp(smoothedHz, hzInput, Time.deltaTime * inputSmoothTime);
        smoothedV = Mathf.Lerp(smoothedV, vInput, Time.deltaTime * inputSmoothTime);

        dir = transform.forward * smoothedV + transform.right * smoothedHz;
        controller.Move(dir * currentMoveSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        spherePos = new Vector3(
            transform.position.x,
            transform.position.y - groundYOffset,
            transform.position.z
        );
        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
    }

    void Gravity()
    {
        if (!IsGrounded())
            velocity.y += gravityValue * Time.deltaTime;
        else if (velocity.y < 0)
            velocity.y = -2f;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }
}