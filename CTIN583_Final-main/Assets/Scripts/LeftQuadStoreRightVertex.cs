using UnityEngine;

public class LeftQuadStoreRightVertex : MonoBehaviour
{
    public QuadSO quadScriptableObject;
    void Start()
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

                    quadScriptableObject.topVertex = transform.TransformPoint(vertices[3]); // Index of upper right vertex
                    quadScriptableObject.bottomVertex = transform.TransformPoint(vertices[1]); // Index of lower right vertex
                    quadScriptableObject.keyPoint = transform.TransformPoint(vertices[0]); // Index of key point
                    quadScriptableObject.midpoint = QuadUtility.GetMidpoint(quadScriptableObject);
                    quadScriptableObject.spawnPoint = QuadUtility.GetMidpoint(quadScriptableObject);

                    // Debug.Log("Top Vertex (Upper Right): " + quadScriptableObject.topVertex);
                    // Debug.Log("Bottom Vertex (Lower Right): " + quadScriptableObject.bottomVertex);
                }
                else
                {
                    Debug.LogWarning("Mesh has no vertices or not enough vertices.");
                }
            }
        }
    }
}
