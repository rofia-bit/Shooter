using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class AimStateManager : MonoBehaviour
{
    [SerializeField] Transform camFollowPos;
    [SerializeField] float sensX = 5f;
    [SerializeField] float sensY = 5f;

    [Header("Aim Zoom")]
    [SerializeField] CinemachineCamera cineCam;
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float aimFOV = 40f;
    [SerializeField] float zoomSpeed = 10f;

    float xRotation;
    float yRotation;

    public Animator anim;

    AimBaseState currentState;
    public HipFireState HipFire = new HipFireState();
    public AimState Aim = new AimState();

    public bool isAiming;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponentInChildren<Animator>();
        SwitchState(HipFire);
    }

    void Update()
    {
        currentState.UpdateState(this);

        var mouse = Mouse.current;
        if (mouse == null) return;

        float mouseX = mouse.delta.x.ReadValue() * sensX * Time.deltaTime;
        float mouseY = mouse.delta.y.ReadValue() * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Smoothly zoom the camera
        float targetFOV = isAiming ? aimFOV : normalFOV;
        cineCam.Lens.FieldOfView = Mathf.Lerp(cineCam.Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(
            xRotation,
            camFollowPos.localEulerAngles.y,
            camFollowPos.localEulerAngles.z
        );

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            yRotation,
            transform.eulerAngles.z
        );
    }
}