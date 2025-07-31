using UnityEngine;

public class PocketMirrorController : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Transform mirrorCamera;
    public GameObject mirrorDisplay;

    [Header("Settings")]
    public float armLength = 1.5f;
    public float rotationSpeed = 50f;
    [Header("Debug")]
    public bool showDebugInfo = false;

    private Vector3 offsetDirection = Vector3.forward;
    private bool isActive = false;
    private Vector3 initialMirrorRotation;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        offsetDirection = Vector3.forward;
        initialMirrorRotation = Vector3.zero;

        SetMirrorActive(false);
        
        if (showDebugInfo)
        {
            if (mirrorCamera != null)
            {
                Camera cam = mirrorCamera.GetComponent<Camera>();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isActive = !isActive;
            SetMirrorActive(isActive);
            
          
        }

        if (!isActive)
            return;

        Vector3 offset = playerCamera.rotation * offsetDirection.normalized * armLength;
        transform.position = playerCamera.position + offset;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            Vector3 rotationInput = new Vector3(-v, h, 0f) * rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationInput, Space.Self);
        }
        if (mirrorCamera != null)
        {
            mirrorCamera.position = transform.position;
            
            Vector3 lookDirection;

            lookDirection = (playerCamera.position - transform.position).normalized;
            
            mirrorCamera.rotation = Quaternion.LookRotation(lookDirection);
            
            if (showDebugInfo && Time.frameCount % 30 == 0)
            {

                Camera cam = mirrorCamera.GetComponent<Camera>();
                if (cam != null)
                {
                }
            }
        }
    }

    private void SetMirrorActive(bool active)
    {
        if (mirrorDisplay != null)
        {
            mirrorDisplay.SetActive(active);
            
            if (showDebugInfo)
            {
                UnityEngine.UI.RawImage rawImage = mirrorDisplay.GetComponent<UnityEngine.UI.RawImage>();
               
            }
        }
       

        if (mirrorCamera != null)
        {
            Camera cameraComponent = mirrorCamera.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                if (active)
                {  
                    Vector3 offset = playerCamera.rotation * offsetDirection.normalized * armLength;
                    mirrorCamera.position = playerCamera.position + offset;
                    
                    
                    Vector3 lookDirection = (playerCamera.position - mirrorCamera.position).normalized;
                    mirrorCamera.rotation = Quaternion.LookRotation(lookDirection);
                    cameraComponent.enabled = true;
                    cameraComponent.Render();
                    
                   
                }
                else
                {
                    cameraComponent.enabled = false;
                    
                   
                }
                
                
            }
            
        }
       
        if (active)
        {
            transform.rotation = playerCamera.rotation;
        }
    }
}