using UnityEngine;

namespace Unrealtor.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayersController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float MoveSpeed = 5f;
        [SerializeField] private float SprintMultiplier = 1.5f;
        [SerializeField] private float Gravity = -9.81f;

        [Header("Camera Settings")]
        [SerializeField] private Transform CameraTransform;
        [SerializeField] private float MouseSensitivity = 2f;

        private CharacterController Controller;
        private float VerticalVelocity = 0f;
        private float XRotation = 0f;
        private bool Inverted = false;

        private CameraRollController CameraRoll;
        private SpyGlassController SpyGlass;

        private void Start()
        {
            Controller = GetComponent<CharacterController>();
            CameraRoll = GetComponent<CameraRollController>();
            SpyGlass = GetComponent<SpyGlassController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            if (Inverted)
            {
                moveX *= -1f;
                moveZ *= -1f;
            }

            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? MoveSpeed * SprintMultiplier : MoveSpeed;

            Vector3 move = (transform.right * moveX + transform.forward * moveZ) * currentSpeed;

            if (Controller.isGrounded && VerticalVelocity < 0)
            {
                VerticalVelocity = -2f;
            }
            else
            {
                VerticalVelocity += Gravity * Time.deltaTime;
            }

            move.y = VerticalVelocity;
            Controller.Move(move * Time.deltaTime);
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

            XRotation -= mouseY;
            XRotation = Mathf.Clamp(XRotation, -65f, 65f);
            transform.Rotate(Vector3.up * mouseX);

            if (CameraRoll != null)
                CameraRoll.UpdateRoll(XRotation);

            if (SpyGlass != null)
                SpyGlass.UpdateZoom();
        }

        public void SetInvertedControls(bool state) => Inverted = state;
        public void ToggleInvertedControls() => Inverted = !Inverted;
    }
}