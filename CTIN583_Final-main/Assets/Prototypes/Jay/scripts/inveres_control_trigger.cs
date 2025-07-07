using UnityEngine;
using UnityEngine.UI;

public class InvertControlToggleZone : MonoBehaviour
{
    public GameObject interactUI;  // Assign your UI Text GameObject in Inspector
    private PlayerController playerController;
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (playerController != null)
            {
                playerController.ToggleInvertedControls();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            playerInZone = true;
            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
            playerInZone = false;
            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}
