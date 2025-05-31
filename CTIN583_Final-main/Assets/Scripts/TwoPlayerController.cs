using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TwoPlayerController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public float moveSpeed = 7f;
    public float maxLookAngle = 60f;
    public float p1LookSensitivity = 50f;
    public float p2LookSensitivity = 100f;
    public float levelChargeTime = 1.5f;
    public float chargeLevel = 0f;
    private float microFactor = 0.15f;
    private float p1Sensitivity; // Adjusted for micro
    private float p2Sensitivity; // Adjusted for micro

    [SerializeField] private Transform player1Camera;
    [SerializeField] private Transform player2Camera;

    public TwoPlayerInput controls;
    private Vector2 player1MovementRaw;
    private Vector2 player2MovementRaw;
    private Vector2 p1Movement; // Adjusted for micro
    private Vector2 p2Movement; // Adjusted for micro
    private Vector2 player1Look;
    private Vector2 player2Look;
    private bool player1Micro;
    private bool player2Micro;
    private float player1Pitch = 0f;
    private float player2Pitch = 0f;

    void Awake()
    {
        // Initialize the PlayerControls
        controls = new TwoPlayerInput();

        // Player 1 movement and look controls
        controls.Player1.Move.performed += ctx => player1MovementRaw = ctx.ReadValue<Vector2>();
        controls.Player1.Move.canceled += ctx => player1MovementRaw = Vector2.zero;
        controls.Player1.Look.performed += ctx => player1Look = ctx.ReadValue<Vector2>();
        controls.Player1.Look.canceled += ctx => player1Look = Vector2.zero;
        controls.Player1.Micro.performed += ctx => player1Micro = true;
        controls.Player1.Micro.canceled += ctx => player1Micro = false;

        // Player 2 movement and look controls
        controls.Player2.Move.performed += ctx => player2MovementRaw = ctx.ReadValue<Vector2>();
        controls.Player2.Move.canceled += ctx => player2MovementRaw = Vector2.zero;
        controls.Player2.Look.performed += ctx => player2Look = ctx.ReadValue<Vector2>();
        controls.Player2.Look.canceled += ctx => player2Look = Vector2.zero;
        controls.Player2.Micro.performed += ctx => player2Micro = true;
        controls.Player2.Micro.canceled += ctx => player2Micro = false;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        SetSensitivity();
        MovePlayer(player1, p1Movement);
        MovePlayer(player2, p2Movement);
        RotatePlayer(player1, player1Look, p1Sensitivity, ref player1Pitch);
        RotatePlayer(player2, player2Look, p2Sensitivity, ref player2Pitch);

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            StartCoroutine(RotateForwardToDown(player1, 0.5f)); // 0.5 seconds smooth transition
        }

    }

    void SetSensitivity()
    {
        if (player1Micro)
        {
            p1Movement = player1MovementRaw * microFactor;
            p1Sensitivity = p1LookSensitivity * microFactor;
        }
        else
        {
            p1Movement = player1MovementRaw;
            p1Sensitivity = p1LookSensitivity;
        }

        if (player2Micro)
        {
            p2Movement = player2MovementRaw * microFactor;
            p2Sensitivity = p2LookSensitivity * microFactor;
        }
        else
        {
            p2Movement = player2MovementRaw;
            p2Sensitivity = p2LookSensitivity;
        }
    }

    void MovePlayer(GameObject player, Vector2 direction)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            // Convert input to 3D movement
            Vector3 move = (player.transform.forward * direction.y + player.transform.right * direction.x) * moveSpeed * Time.deltaTime;
            controller.Move(move);
        }
    }

    void RotatePlayer(GameObject player, Vector2 lookDirection, float lookSensitivity, ref float pitch)
    {
        // Yaw rotation (left/right) around *local up*, not world up
        float yaw = lookDirection.x * lookSensitivity * Time.deltaTime;
        player.transform.Rotate(player.transform.up, yaw, Space.World);

        // Only update pitch if lookDirection.y is not zero
        if (lookDirection.y != 0)
        {
            pitch += -lookDirection.y * lookSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        }

        // Apply pitch to the camera locally (rotating around the local right axis)
        if (player == player1 && player1Camera != null)
        {
            player1Camera.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
        }

        if (player == player2 && player2Camera != null)
        {
            player2Camera.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
        }
    }

    IEnumerator RotateForwardToDown(GameObject player, float duration)
    {
        // Cast a ray forward from the player to detect the surface
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 10f))  // Adjust raycast distance as needed
        {
            // Use the normal of the hit surface as the new "up" direction
            Vector3 newUp = hit.normal;

            // Store the current rotation of the player
            Quaternion startRotation = player.transform.rotation;

            // Create the rotation needed to align the player's "up" vector with the surface normal
            Quaternion endRotation = Quaternion.FromToRotation(player.transform.up, newUp) * player.transform.rotation;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                // Smoothly rotate from start to end rotation over time
                float t = elapsed / duration;
                player.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the final rotation is exactly aligned with the surface normal
            player.transform.rotation = endRotation;
        }
    }

}
