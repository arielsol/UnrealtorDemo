using UnityEngine;

public class PillarTrigger3D : MonoBehaviour
{
    public Transform pillar;                  // The pillar object to move
    public Vector3 loweredOffset = Vector3.down;
    public float speed = 2f;

    private Vector3 originalPos;
    private Vector3 targetPos;
    private bool playerNear = false;

    void Start()
    {
        if (pillar == null) pillar = transform.Find("Pillar");
        originalPos = pillar.position;
    }

    void Update()
    {
        targetPos = playerNear ? originalPos + loweredOffset : originalPos;
        pillar.position = Vector3.MoveTowards(pillar.position, targetPos, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
