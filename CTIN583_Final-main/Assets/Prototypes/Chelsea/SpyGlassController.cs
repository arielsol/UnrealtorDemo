using UnityEngine;

namespace Unrealtor.Player
{
    public class SpyGlassController : MonoBehaviour
    {
        [Header("SpyGlass Visual")]
        [SerializeField] private Transform CameraTransform;
        [SerializeField] private Camera PlayerCamera;
        [SerializeField] private GameObject SpyGlassPrefab;
        [SerializeField] private Vector3 HeldOffset = new Vector3(0.05f, -0.05f, 0.25f);
        [SerializeField] private float RaiseSpeed = 10f;

        [Header("Zoom Settings")]
        [SerializeField] private float ZoomSpeed = 5f;

        private readonly float[] ZoomFOVs = new float[] { 73.74f, 39.6f, 10.3f };
        private int CurrentZoomIndex = 0;
        private bool UsingSpyGlass = false;

        private GameObject SpyGlassInstance;
        private Transform SpyGlassTransform;

        private void Start()
        {
            if (SpyGlassPrefab != null && CameraTransform != null)
            {
                SpyGlassInstance = Instantiate(SpyGlassPrefab);
                SpyGlassTransform = SpyGlassInstance.transform;

                SpyGlassTransform.position = CameraTransform.position + CameraTransform.TransformVector(HeldOffset);
                SpyGlassTransform.rotation = CameraTransform.rotation;
                SpyGlassTransform.localScale = Vector3.one;

                SpyGlassInstance.SetActive(false);//disable spyglass at beginning
            }
        }

        private void Update()
        {
            bool holdingCtrlOrCmd =
                Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

            if (holdingCtrlOrCmd && !UsingSpyGlass)
            {
                UsingSpyGlass = true;
                SetSpyGlass(true);
            }
            else if (!holdingCtrlOrCmd && UsingSpyGlass)
            {
                UsingSpyGlass = false;
                SetSpyGlass(false);
            }

            if (UsingSpyGlass)
            {
                MaintainSpyGlassPosition();
                UpdateZoom();
            }
            else
            {
                if (PlayerCamera != null)
                    PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, 60f, Time.deltaTime * ZoomSpeed);
            }
        }

        private void MaintainSpyGlassPosition()
        {
            if (SpyGlassTransform == null || CameraTransform == null) return;

            Vector3 targetPosition = CameraTransform.TransformPoint(HeldOffset);
            SpyGlassTransform.position = Vector3.Lerp(SpyGlassTransform.position, targetPosition, Time.deltaTime * RaiseSpeed);
            SpyGlassTransform.rotation = CameraTransform.rotation;
        }

        public void UpdateZoom()
        {
            if (PlayerCamera == null || SpyGlassInstance == null) return;

            float targetFOV = 60f;

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput > 0f)
                CurrentZoomIndex = (CurrentZoomIndex + 1) % ZoomFOVs.Length;
            else if (scrollInput < 0f)
                CurrentZoomIndex = (CurrentZoomIndex - 1 + ZoomFOVs.Length) % ZoomFOVs.Length;

            if (Input.GetKeyDown(KeyCode.Alpha1)) CurrentZoomIndex = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) CurrentZoomIndex = 1;
            if (Input.GetKeyDown(KeyCode.Alpha5)) CurrentZoomIndex = 2;

            targetFOV = ZoomFOVs[CurrentZoomIndex];

            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, targetFOV, Time.deltaTime * ZoomSpeed);
        }

        private void SetSpyGlass(bool state)
        {
            if (SpyGlassInstance != null)
                SpyGlassInstance.SetActive(state);
        }
    }
}