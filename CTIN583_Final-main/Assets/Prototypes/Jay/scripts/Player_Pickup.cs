using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 5f;
    public Transform holdPoint; // e.g. empty GameObject in front of camera

    [Header("Layer Masks")]
    public LayerMask itemLayer;
    public LayerMask placementZoneLayer;

    private GameObject heldItem = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldItem == null)
            {
                TryPickupItem();
            }
            else
            {
                TryPlaceItem();
            }
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupRange, Color.red);

    }

    void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, itemLayer))
        {
            Debug.Log("🎯 Ray hit: " + hit.collider.name);

            GameObject item = hit.collider.gameObject;
            Rigidbody rb = item.GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogWarning("❌ No Rigidbody on " + item.name);
                return;
            }

            //rb.isKinematic = true;

            item.transform.SetParent(holdPoint);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            heldItem = item;
            Debug.Log("👐 Picked up: " + item.name);
        }
        else
        {
            Debug.Log("❌ No object hit in pickup range.");
        }
    }


    void TryPlaceItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, placementZoneLayer))
        {
            Transform placeTarget = hit.collider.transform;

            // Unparent and move to placement target
            heldItem.transform.SetParent(null);
            heldItem.transform.position = placeTarget.position;
            heldItem.transform.rotation = placeTarget.rotation;

            // Re-enable physics
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            //if (rb) rb.isKinematic = false;

            Debug.Log("📦 Placed at: " + placeTarget.name);
            heldItem = null;
        }
        else
        {
            Debug.Log("❌ Not looking at valid placement zone.");
        }
    }
}
