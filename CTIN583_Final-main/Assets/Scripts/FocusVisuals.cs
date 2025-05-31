using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FocusVisuals : MonoBehaviour
{
    [SerializeField] private Volume player1Volume;
    [SerializeField] private Volume player2Volume;
    [SerializeField] private TextMeshProUGUI[] textLeft;
    [SerializeField] private TextMeshProUGUI[] textRight;
    private Vignette p1Vignette;
    private Vignette p2Vignette;
    private TwoPlayerInput controls;
    private float currentTime1 = 0f;
    private float currentTime2 = 0f;
    public float vignetteIntensity = 0.31f;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private bool isPlayer1Holding = false;
    private bool isPlayer2Holding = false;

    void Awake()
    {
        controls = new TwoPlayerInput();

        controls.Player1.Micro.performed += ctx => OnPlayer1Micro();
        controls.Player2.Micro.performed += ctx => OnPlayer2Micro();
        controls.Player1.Micro.canceled += ctx => OnPlayer1MicroOff();
        controls.Player2.Micro.canceled += ctx => OnPlayer2MicroOff();

        player1Volume.profile.TryGet(out p1Vignette);
        player2Volume.profile.TryGet(out p2Vignette);
    }

    void Start()
    {
        p1Vignette.intensity.value = 0f;
        p2Vignette.intensity.value = 0f;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnPlayer1Micro()
    {
        isPlayer1Holding = true;
        currentTime1 = 0f; // Reset the fade time when the button is pressed
    }

    void OnPlayer1MicroOff()
    {
        isPlayer1Holding = false;
        currentTime1 = 0f; // Reset the fade time when the button is released
    }

    void OnPlayer2Micro()
    {
        isPlayer2Holding = true;
        currentTime2 = 0f; // Reset the fade time when the button is pressed
    }

    void OnPlayer2MicroOff()
    {
        isPlayer2Holding = false;
        currentTime2 = 0f; // Reset the fade time when the button is released
    }

    void Update()
    {
        // Player 1 Fade In/Out
        if (isPlayer1Holding)
        {
            // Fade in while button is held
            if (currentTime1 < fadeInTime)
            {
                float t = currentTime1 / fadeInTime;
                p1Vignette.intensity.value = Mathf.Lerp(0f, vignetteIntensity, t);
                foreach (TextMeshProUGUI i in textLeft)
                {
                    float alpha = Mathf.Lerp(1f, 0f, t);
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, alpha);
                }
                currentTime1 += Time.deltaTime;
            }
            else
            {
                p1Vignette.intensity.value = vignetteIntensity; // Ensure max intensity is set
                foreach (TextMeshProUGUI i in textLeft)
                {
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, 0f);
                }
            }
        }
        else
        {
            // Fade out when button is released
            if (currentTime1 < fadeOutTime)
            {
                float t = currentTime1 / fadeOutTime;
                p1Vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0f, t);
                foreach (TextMeshProUGUI i in textLeft)
                {
                    float alpha = Mathf.Lerp(0f, 1f, t);
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, alpha);
                }
                currentTime1 += Time.deltaTime;
            }
            else
            {
                p1Vignette.intensity.value = 0f; // Ensure intensity is fully off
                foreach (TextMeshProUGUI i in textLeft)
                {
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, 1f);
                }
            }
        }

        // Player 2 Fade In/Out
        if (isPlayer2Holding)
        {
            // Fade in vignette while button is held
            if (currentTime2 < fadeInTime)
            {
                float t = currentTime2 / fadeInTime;
                p2Vignette.intensity.value = Mathf.Lerp(0f, vignetteIntensity, t);
                foreach (TextMeshProUGUI i in textRight)
                {
                    float alpha = Mathf.Lerp(1f, 0f, t);
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, alpha);
                }
                currentTime2 += Time.deltaTime;
            }
            else
            {
                p2Vignette.intensity.value = vignetteIntensity; // Ensure max intensity is set
                foreach (TextMeshProUGUI i in textRight)
                {
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, 0f);
                }
            }
        }
        else
        {
            // Fade out when button is released
            if (currentTime2 < fadeOutTime)
            {
                float t = currentTime2 / fadeOutTime;
                p2Vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0f, t);
                foreach (TextMeshProUGUI i in textRight)
                {
                    float alpha = Mathf.Lerp(0f, 1f, t);
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, alpha);
                }
                currentTime2 += Time.deltaTime;
            }
            else
            {
                p2Vignette.intensity.value = 0f; // Ensure intensity is fully off
                foreach (TextMeshProUGUI i in textRight)
                {
                    Color color = i.color;
                    i.color = new Color(color.r, color.g, color.b, 1f);
                }
            }
        }
    }
}
