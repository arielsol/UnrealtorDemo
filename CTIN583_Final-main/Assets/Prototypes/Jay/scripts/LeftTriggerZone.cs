using UnityEngine;

public class LeftTriggerZone : MonoBehaviour
{
    public DualTriggerMatchManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            manager.leftPlayerInZone = true;
            manager.leftPlayer = other.transform;
            Debug.Log("👈 Player entered LEFT trigger zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (manager.leftPlayer == other.transform)
            {
                manager.leftPlayerInZone = false;
                manager.leftPlayer = null;
                Debug.Log("👈 Player exited LEFT trigger zone");
            }
        }
    }
}
