using UnityEngine;

public class RightTriggerZone : MonoBehaviour
{
    public DualTriggerMatchManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            manager.rightPlayerInZone = true;
            manager.rightPlayer = other.transform;
            Debug.Log("👉 Player entered RIGHT trigger zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (manager.rightPlayer == other.transform)
            {
                manager.rightPlayerInZone = false;
                manager.rightPlayer = null;
                Debug.Log("👉 Player exited RIGHT trigger zone");
            }
        }
    }
}
