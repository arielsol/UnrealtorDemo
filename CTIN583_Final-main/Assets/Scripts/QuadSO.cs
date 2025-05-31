using UnityEngine;

[CreateAssetMenu(fileName = "NewQuadSO", menuName = "ScriptableObjects/QuadSO")]
public class QuadSO : ScriptableObject
{
    // Stores the vertex points as Vector3
    public Vector3 topVertex;
    public Vector3 bottomVertex;
    public Vector3 midpoint;
    public Vector3 keyPoint;

    public Vector3 spawnPoint;
    public Vector3 size;
}
