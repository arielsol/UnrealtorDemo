using UnityEngine;

public class QuitOnEscape : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // Stop play mode if in the editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the application in build
            Application.Quit();
#endif
        }
    }
}
