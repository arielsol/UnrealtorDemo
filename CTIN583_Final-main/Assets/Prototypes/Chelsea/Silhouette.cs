using UnityEngine;

public class Silhouette : MonoBehaviour
{
    [Tooltip("Assign a black, semi-transparent material for 3D objects.")]
    public Material silhouetteMaterial3D;
    [Tooltip("Set the color and alpha for 2D sprites.")]
    public Color silhouetteColor2D = new Color(0, 0, 0, 0.5f);

    private Material[] originalMaterials;
    private Color originalSpriteColor;
    private Renderer meshRenderer;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (meshRenderer != null && !(meshRenderer is SpriteRenderer))
        {
            originalMaterials = meshRenderer.sharedMaterials;
        }
        if (spriteRenderer != null)
        {
            originalSpriteColor = spriteRenderer.color;
        }
        EnableSilhouette();
    }

    public void EnableSilhouette()
    {
        if (meshRenderer != null && silhouetteMaterial3D != null && !(meshRenderer is SpriteRenderer))
        {
            Material[] mats = new Material[meshRenderer.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = silhouetteMaterial3D;
            meshRenderer.materials = mats;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = silhouetteColor2D;
        }
    }
    
    public void DisableSilhouette()
    {
        if (meshRenderer != null && originalMaterials != null && !(meshRenderer is SpriteRenderer))
        {
            meshRenderer.materials = originalMaterials;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalSpriteColor;
        }
    }
}
