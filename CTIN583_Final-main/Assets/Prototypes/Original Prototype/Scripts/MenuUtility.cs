using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUtility : MonoBehaviour
{
    private float switchVolume1;
    private float switchVolume2;

    private void Awake()
    {
        Cursor.visible = true;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(1);

        switchVolume1 = AudioManager.Instance.Audio1.volume;
        switchVolume2 = AudioManager.Instance.Audio2.volume;

        AudioManager.Instance.Audio1.volume = switchVolume2;
        AudioManager.Instance.Audio2.volume = switchVolume1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
