using UnityEngine;

public class DualTriggerMatchManager : MonoBehaviour
{
    [Header("Player State")]
    public bool leftPlayerInZone = false;
    public bool rightPlayerInZone = false;

    public Transform leftPlayer;
    public Transform rightPlayer;

    [Header("Plane Normal Check")]
    public Transform referencePlane;
    public float angleThreshold = 20f;

    [Header("Objects To Toggle")]
    public GameObject obj1;
    public GameObject obj2;

    [Header("Match Settings")]
    public float holdTime = 3f;

    private float matchTimer = 0f;
    private bool isMatched = false;

    void Update()
    {
        if (leftPlayerInZone && rightPlayerInZone && PlayersFacingNormal())
        {
            matchTimer += Time.deltaTime;

            if (matchTimer >= holdTime && !isMatched)
            {
                ToggleObjects();
                isMatched = true;
                Debug.Log("✅ Matched conditions held for 3 seconds. Objects toggled.");
            }
        }
        else
        {
            if (matchTimer > 0)
                Debug.Log("⏱️ Conditions broken. Timer reset.");

            matchTimer = 0f;

            if (isMatched)
            {
                //ToggleObjects();
                isMatched = false;
                Debug.Log("🔄 Conditions failed. Objects shown again.");
            }
        }
    }

    bool PlayersFacingNormal()
    {
        if (leftPlayer == null || rightPlayer == null || referencePlane == null)
            return false;

        Vector3 planeNormal = -referencePlane.transform.up;

        float leftDot = Vector3.Dot(leftPlayer.forward.normalized, planeNormal);
        float rightDot = Vector3.Dot(rightPlayer.forward.normalized, planeNormal);

        float leftAngle = Mathf.Acos(leftDot) * Mathf.Rad2Deg;
        float rightAngle = Mathf.Acos(rightDot) * Mathf.Rad2Deg;

        Debug.Log($"🎯 Left angle: {leftAngle:F1}, Right angle: {rightAngle:F1}");

        return leftAngle < angleThreshold && rightAngle < angleThreshold;
    }

    void ToggleObjects()
    {
        if (obj1) obj1.SetActive(!obj1.activeSelf);
        if (obj2) obj2.SetActive(!obj2.activeSelf);
    }
}
