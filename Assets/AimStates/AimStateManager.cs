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

    [Header("Aim Raycast")]
    [SerializeField] Transform aimPos;
    [SerializeField] float aimSmoothSpeed = 10f;
    [SerializeField] LayerMask aimMask;

    float xRotation;
    float yRotation;

    [SerializeField] public Animator anim;

    AimBaseState currentState;
    public HipFireState HipFire = new HipFireState();
    public AimState Aim = new AimState();

    public bool isAiming;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (cineCam != null) normalFOV = cineCam.Lens.FieldOfView;

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

        if (cineCam != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            cineCam.Lens.FieldOfView = Mathf.Lerp(cineCam.Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        }

        if (aimPos != null)
        {
            Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
                aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
            else
                aimPos.position = Vector3.Lerp(aimPos.position, ray.GetPoint(100), aimSmoothSpeed * Time.deltaTime);
        }
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void LateUpdate()
    {
        if (camFollowPos != null)
        {
            camFollowPos.localEulerAngles = new Vector3(
                xRotation,
                camFollowPos.localEulerAngles.y,
                camFollowPos.localEulerAngles.z
            );
        }

        // Only rotate Y axis — prevents sideways body tilt
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}