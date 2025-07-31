using UnityEngine;

public class CameraFOVToggle : MonoBehaviour
{
    public Camera targetCamera;
    public float fovA = 30f;
    public float fovB = 60f;

    private bool usingFovA = true;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetCamera.fieldOfView = fovA;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            usingFovA = !usingFovA;
            targetCamera.fieldOfView = usingFovA ? fovA : fovB;
            Debug.Log("🎥 FOV toggled to: " + targetCamera.fieldOfView);
        }
    }
}
