using UnityEngine;

public class RightQuadStoreLeftVertex : MonoBehaviour
{
    public QuadSO quadScriptableObject;

    void Awake()
    {
        if (quadScriptableObject != null)
        {
            // Get the MeshFilter component to access the mesh
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;

                // Ensure the mesh has vertices
                if (mesh != null && mesh.vertexCount >= 4)
                {
                    // Get the vertices of the mesh
                    Vector3[] vertices = mesh.vertices;

                    // Store in the ScriptableObject
                    quadScriptableObject.topVertex = transform.TransformPoint(vertices[2]); // Index of upper left vertex
                    quadScriptableObject.bottomVertex = transform.TransformPoint(vertices[0]); // Index of lower left vertex
                    quadScriptableObject.keyPoint = transform.TransformPoint(vertices[1]); // Index of key point
                    quadScriptableObject.midpoint = QuadUtility.GetMidpoint(quadScriptableObject);
                    quadScriptableObject.spawnPoint = QuadUtility.GetMidpoint(quadScriptableObject);

                    // Debug.Log("Top Vertex (Upper Left): " + quadScriptableObject.topVertex);
                    // Debug.Log("Bottom Vertex (Lower Left): " + quadScriptableObject.bottomVertex);
                }
                else
                {
                    Debug.LogWarning("Mesh has no vertices or not enough vertices.");
                }
            }
        }
    }
}
