using UnityEngine;

public class ItemAndPlayerTriggerToggleLight : MonoBehaviour
{
    [Header("References")]
    public GameObject targetItem;         // Item to detect
    public GameObject player;             // Player to detect
    public Light spotlight;               // Spotlight to toggle

    [Header("Toggle Colors")]
    public Color colorA = Color.red;
    public Color colorB = Color.green;

    private bool itemInside = false;
    private bool playerInside = false;
    private bool useColorA = true;

    void Update()
    {
        if (itemInside && playerInside && Input.GetKeyDown(KeyCode.T))
        {
            spotlight.color = useColorA ? colorA : colorB;
            useColorA = !useColorA;
            Debug.Log("🔦 Spotlight color toggled.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetItem)
        {
            itemInside = true;
            Debug.Log("✅ Item entered trigger zone.");
        }
        else if (other.gameObject == player)
        {
            playerInside = true;
            Debug.Log("✅ Player entered trigger zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetItem)
        {
            itemInside = false;
            Debug.Log("⛔ Item exited trigger zone.");
        }
        else if (other.gameObject == player)
        {
            playerInside = false;
            Debug.Log("⛔ Player exited trigger zone.");
        }
    }
}
