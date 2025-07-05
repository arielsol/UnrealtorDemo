using UnityEngine;

public class PlayerGazeTimerTrigger : MonoBehaviour
{
    [Header("References")]
    private Transform playerCamera;
    private Transform currentPlayer;
    public Transform targetObject;         // The object to look at

    [Header("Snap Target (World Space)")]
    public Vector3 snapPosition;           // Position to move player to

    [Header("Detection Settings")]
    public float lookAngleThreshold = 15f;
    public float requiredGazeTime = 3f;

    [Header("Objects to Toggle")]
    public GameObject obj1;                // Starts visible
    public GameObject obj2;                // Starts hidden

    private bool playerInside = false;
    private float gazeTimer = 0f;
    private bool triggered = false;

    void Update()
    {
        if (!playerInside || playerCamera == null || targetObject == null || triggered)
            return;

        Vector3 toTarget = (targetObject.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, toTarget);

        if (angle <= lookAngleThreshold)
        {
            gazeTimer += Time.deltaTime;

            if (gazeTimer >= requiredGazeTime)
            {
                TriggerSuccess();
                triggered = true;
            }
        }
        else
        {
            gazeTimer = 0f;
        }
    }

    void TriggerSuccess()
    {
        Debug.Log("✅ Gaze condition met.");
        if (currentPlayer != null)
        {
            Debug.Log("📍 Player position before snap: " + currentPlayer.position);

            CharacterController controller = currentPlayer.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                currentPlayer.position = snapPosition;
                controller.enabled = true;
            }
        }

        // Rotate camera to look at target
        if (playerCamera != null && targetObject != null)
        {
            Vector3 direction = (targetObject.position - playerCamera.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = lookRotation;
        }

        // Toggle visibility
        if (obj1 != null) obj1.SetActive(!obj1.activeSelf);
        if (obj2 != null) obj2.SetActive(!obj2.activeSelf);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            currentPlayer = other.transform;

            // Only find camera by exact name to avoid picking overlays
            Transform camLeft = currentPlayer.Find("CameraLeft");
            Transform camRight = currentPlayer.Find("CameraRight");

            if (camLeft != null)
                playerCamera = camLeft;
            else if (camRight != null)
                playerCamera = camRight;
            else
                Debug.LogWarning("🚨 Could not find CameraLeft or CameraRight under " + currentPlayer.name);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == currentPlayer)
        {
            playerInside = false;
            currentPlayer = null;
            playerCamera = null;
            gazeTimer = 0f;
            triggered = false;
        }
    }

}
