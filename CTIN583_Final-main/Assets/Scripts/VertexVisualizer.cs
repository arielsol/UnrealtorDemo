using UnityEngine;
using System.Collections.Generic;

public class VertexVisualizer : MonoBehaviour
{
    public List<QuadSO> quadDataList; // List of quadSO Scriptable Objects
    public float sphereSize = 0.1f;
    public Color vertexColor = Color.red;
    public Color areaColor = Color.green;

    private Mesh sphereMesh;
    private Material vertexMaterial;

    void Start()
    {
        // Create a sphere mesh and material for drawing
        sphereMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(GameObject.CreatePrimitive(PrimitiveType.Sphere)); // Clean up temporary sphere
        vertexMaterial = new Material(Shader.Find("Standard"));
    }

    void OnRenderObject()
    {
        if (quadDataList == null || quadDataList.Count == 0 || sphereMesh == null || vertexMaterial == null)
            return;

        foreach (var quadData in quadDataList)
        {
            if (quadData == null) continue;

            // Set the color of the material for the vertex
            vertexMaterial.color = vertexColor;
            vertexMaterial.SetPass(0);

            // Draw sphere at the top vertex
            Graphics.DrawMeshNow(sphereMesh, Matrix4x4.TRS(quadData.topVertex, Quaternion.identity, Vector3.one * sphereSize));

            // Draw sphere at the bottom vertex
            Graphics.DrawMeshNow(sphereMesh, Matrix4x4.TRS(quadData.bottomVertex, Quaternion.identity, Vector3.one * sphereSize));

            // Draw sphere at the spawn point
            Graphics.DrawMeshNow(sphereMesh, Matrix4x4.TRS(quadData.spawnPoint, Quaternion.identity, Vector3.one * sphereSize));

            // Draw sphere at check area center
            // vertexMaterial.color = areaColor;
            // vertexMaterial.SetPass(0);
            // Graphics.DrawMeshNow(sphereMesh, Matrix4x4.TRS(quadData.checkAreaCenter, Quaternion.identity, Vector3.one * quadData.checkAreaRadius * 2f));
            // Not sure why the *2 is needed, but maybe not important
        }
    }

    void OnDrawGizmos()
    {
        if (quadDataList == null || quadDataList.Count == 0)
            return;

        foreach (var quadData in quadDataList)
        {
            if (quadData == null) continue;

            // Set Gizmo color for vertices
            Gizmos.color = vertexColor;

            // Draw gizmos for top and bottom vertices
            Gizmos.DrawSphere(quadData.topVertex, sphereSize);
            Gizmos.DrawSphere(quadData.bottomVertex, sphereSize);
            Gizmos.DrawSphere(quadData.spawnPoint, sphereSize);

            // Set Gizmo color for check area center
            Gizmos.color = areaColor;
            // Gizmos.DrawSphere(quadData.checkAreaCenter, quadData.checkAreaRadius);
        }
    }
}
