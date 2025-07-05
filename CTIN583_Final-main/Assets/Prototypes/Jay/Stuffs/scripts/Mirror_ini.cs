using UnityEngine;

public class SimpleMirrorSetup : MonoBehaviour
{
    public Camera mirrorCamera;          // The camera rendering the mirror view
    public Renderer mirrorSurface;       // The surface displaying the reflection
    public int textureWidth = 512;
    public int textureHeight = 512;

    void Start()
    {
        if (!mirrorCamera || !mirrorSurface) return;

        // Create a RenderTexture
        RenderTexture renderTex = new RenderTexture(textureWidth, textureHeight, 16);
        renderTex.name = "SimpleMirrorRT_" + gameObject.name;

        // Assign it to the camera
        mirrorCamera.targetTexture = renderTex;

        // Apply it to the mirror surface material
        Material mat = new Material(mirrorSurface.sharedMaterial);
        mat.mainTexture = renderTex;
        mirrorSurface.material = mat;
    }
}
