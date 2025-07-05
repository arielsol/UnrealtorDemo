using UnityEngine;
using UnityEngine.Rendering;

public class AmbientLightAndSpotControl : MonoBehaviour
{
    [Header("Flat Color Settings")]
    public Color flatAmbientColor = Color.black;

    [Header("Skybox Settings")]
    public Material skyboxMaterial;
    public float skyboxAmbientIntensity = 1f;

    [Header("Directional Light")]
    public Light directionalLight;  // Main sun light (toggle on/off)

    [Header("Spotlight to Control")]
    public Light spotlight;         // Spotlight to color (should be type Spot)

    private bool usingSkybox = true;

    void Start()
    {
        ApplySkyboxMode();
    }

    void Update()
    {
        // Toggle skybox vs flat ambient
        if (Input.GetKeyDown(KeyCode.L))
        {
            usingSkybox = !usingSkybox;

            if (usingSkybox)
                ApplySkyboxMode();
            else
                ApplyBlackoutMode();
        }

        // Set spotlight color
        if (spotlight != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                spotlight.color = Color.red;
                Debug.Log("🔴 Spotlight set to RED");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                spotlight.color = Color.green;
                Debug.Log("🟢 Spotlight set to GREEN");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                spotlight.color = Color.blue;
                Debug.Log("🔵 Spotlight set to BLUE");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                spotlight.color = Color.white;
                Debug.Log("⚪ Spotlight set to WHITE");
            }
        }
    }

    void ApplySkyboxMode()
    {
        // Use default skybox if none assigned
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }
        else
        {
            Debug.LogWarning("⚠️ No skyboxMaterial assigned! Default skybox will be used if set in Lighting settings.");
            // Do NOT overwrite RenderSettings.skybox if null
        }

        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = skyboxAmbientIntensity;
        RenderSettings.ambientLight = Color.white;

        if (Camera.main != null)
            Camera.main.clearFlags = CameraClearFlags.Skybox;

        if (directionalLight != null)
            directionalLight.enabled = true;

        DynamicGI.UpdateEnvironment();

        Debug.Log("✅ Skybox lighting and directional light enabled.");
    }



    void ApplyBlackoutMode()
    {
        // Lighting
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = flatAmbientColor;
        RenderSettings.skybox = null;

        // Camera settings
        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
        }

        // Light
        if (directionalLight != null)
            directionalLight.enabled = false;

        Debug.Log("🌑 Switched to blackout mode: no skybox, flat ambient, black background.");
    }
}
