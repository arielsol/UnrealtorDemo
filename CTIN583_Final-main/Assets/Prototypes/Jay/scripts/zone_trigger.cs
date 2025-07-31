using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public string playerTag = "Player1"; // Set to "Player1" or "Player2" in Inspector
    public MatchCoordinator matchManager;    // Reference to the central manager

    void Start()
    {
        matchManager = FindObjectOfType<MatchCoordinator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"✅ {playerTag} entered {gameObject.name}");
            matchManager.SetPlayerInZone(playerTag, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"❌ {playerTag} exited {gameObject.name}");
            matchManager.SetPlayerInZone(playerTag, false);
        }
    }
}
