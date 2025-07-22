using UnityEngine;

public class MatchCoordinator : MonoBehaviour
{
    [Header("Player References")]
    public Transform playerLeft;
    public Transform playerRight;
    public Camera cameraLeft;
    public Camera cameraRight;

    [Header("Look At Targets")]
    public Transform lookAtLeft;
    public Transform lookAtRight;

    [Header("Match Settings")]
    public float lookAngleThreshold = 15f;
    public float distanceTolerance = 0.5f;

    [Header("Object Scale References")]
    public Transform objectLeft;
    public Transform objectRight;

    [Header("UI Feedback")]
    public GameObject matchedUI;

    private bool isLeftInZone = false;
    private bool isRightInZone = false;

    void Update()
    {
        CheckMatchCondition();
    }

    public void SetPlayerInZone(string tag, bool isInside)
    {
        if (tag == "Player1")
            isLeftInZone = isInside;
        else if (tag == "Player2")
            isRightInZone = isInside;
    }

    void CheckMatchCondition()
    {
        if (!isLeftInZone || !isRightInZone)
        {
            if (matchedUI != null)
                matchedUI.SetActive(false);
            return;
        }

        // Check if both players are looking at their respective look-at targets
        bool leftLooking = IsLookingAt(cameraLeft.transform, lookAtLeft);
        bool rightLooking = IsLookingAt(cameraRight.transform, lookAtRight);

        Debug.Log("👁️ Left looking: " + leftLooking + ", Right looking: " + rightLooking);

        if (!leftLooking || !rightLooking)
        {
            if (matchedUI != null)
                matchedUI.SetActive(false);
            Debug.Log("❌ One or both players are not looking at the correct object.");
            return;
        }

        // Check distance alignment based on object scale
        float scaleLeft = objectLeft.localScale.x;
        float scaleRight = objectRight.localScale.x;

        float distLeft = Vector3.Distance(playerLeft.position, objectLeft.position);
        float distRight = Vector3.Distance(playerRight.position, objectRight.position);

        float expectedRatio = scaleLeft / scaleRight;
        float actualRatio = distLeft / distRight;

        if (Mathf.Abs(expectedRatio - actualRatio) <= distanceTolerance)
        {
            Debug.Log("✅ Matched! Both players are in position, looking, and aligned.");
            if (matchedUI != null)
                matchedUI.SetActive(true);
        }
        else
        {
            Debug.Log("❌ Distances not properly aligned based on scale.");
            if (matchedUI != null)
                matchedUI.SetActive(false);
        }
    }

    bool IsLookingAt(Transform eye, Transform target)
    {
        Vector3 toTarget = (target.position - eye.position).normalized;
        float angle = Vector3.Angle(eye.forward, toTarget);
        return angle <= lookAngleThreshold;
    }
}
