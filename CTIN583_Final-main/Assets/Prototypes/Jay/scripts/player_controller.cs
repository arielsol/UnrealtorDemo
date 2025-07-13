using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;              // Normal speed
    public float sprintMultiplier = 1.5f;     // Speed multiplier when sprinting
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    private float verticalVelocity = 0f;
    private float xRotation = 0f;
    private CharacterController controller;
    private bool inverted = false;

    [Header("Extra Camera Controls")]
    [SerializeField] private Camera playerCam;
    public float defaultFOV = 60f;
    public float zoomFOV = 40f;
    public float fovZoomSpeed = 10f;

    public float rollSpeed = 90f;     // Degrees per second
    public float rollReturnSpeed = 180f; // Speed when returning to upright

    private float currentRoll = 0f;
    private float targetRoll = 0f;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (inverted)
        {
            moveX *= -1f;
            moveZ *= -1f;
        }

        // Check for sprint input
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= currentSpeed;

        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -65f, 65f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        HandleCameraZoom();
        HandleCameraRoll();
    }


    public void SetInvertedControls(bool state)
    {
        inverted = state;
    }

    public void ToggleInvertedControls()
    {
        inverted = !inverted;
    }
    void HandleCameraZoom()
    {
        if (playerCam == null) return;

        float targetFOV = Input.GetMouseButton(1) ? zoomFOV : defaultFOV;
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * fovZoomSpeed);
    }

    void HandleCameraRoll()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.Q)) input += 1f;
        if (Input.GetKey(KeyCode.E)) input -= 1f;

        // Set target roll direction
        if (input != 0f)
            targetRoll += input * rollSpeed * Time.deltaTime;
        else
            targetRoll = Mathf.Lerp(targetRoll, 0f, Time.deltaTime * (rollReturnSpeed / 90f)); // Smooth return

        currentRoll = Mathf.Lerp(currentRoll, targetRoll, Time.deltaTime * 10f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, currentRoll);
    }


}
