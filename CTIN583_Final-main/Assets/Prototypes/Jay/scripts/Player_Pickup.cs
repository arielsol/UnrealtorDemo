using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 5f;
    public Transform holdPoint; // e.g. empty GameObject in front of camera

    [Header("Layer Masks")]
    public LayerMask itemLayerA;
    public LayerMask itemLayerB;
    public LayerMask placementZoneLayer;

    private GameObject heldItem = null;
    private ItemType heldItemType;

    private enum ItemType { None, TypeA, TypeB }

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

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupRange, Color.red);
    }

    void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, itemLayerA))
        {
            Pickup(hit.collider.gameObject, ItemType.TypeA);
        }
        else if (Physics.Raycast(ray, out hit, pickupRange, itemLayerB))
        {
            Pickup(hit.collider.gameObject, ItemType.TypeB);
        }
        else
        {
            Debug.Log("❌ No item detected in pickup range.");
        }
    }

    void Pickup(GameObject item, ItemType type)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.isKinematic = true;
        rb.useGravity = false;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        heldItem = item;
        heldItemType = type;

        Debug.Log("👐 Picked up " + type + ": " + item.name);
    }

    void TryPlaceItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, placementZoneLayer))
        {
            // Only allow placement if holding TypeA
            if (heldItemType != ItemType.TypeA)
            {
                Debug.Log("❌ Cannot place this item type here.");
                return;
            }

            Transform placeTarget = hit.collider.transform;

            heldItem.transform.SetParent(null);
            heldItem.transform.position = placeTarget.position;
            heldItem.transform.rotation = placeTarget.rotation;

            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            rb.isKinematic = true;  // Stay in place, no bounce
            rb.useGravity = false;

            Debug.Log("📦 Placed at: " + placeTarget.name);
            heldItem = null;
            heldItemType = ItemType.None;
        }
        else
        {
            Debug.Log("❌ Not looking at a valid placement zone.");
        }
    }

    void DropItem()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null);

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            //rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
        }

        Debug.Log("📤 Dropped: " + heldItem.name);

        heldItem = null;
        heldItemType = ItemType.None;
    }
}
