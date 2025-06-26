using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInText : MonoBehaviour
{

    [SerializeField] private float fadeInTime = 1.5f;
    private TextMeshProUGUI text;
    private float fadeTimer;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, 0f);

        fadeTimer = 0f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (fadeTimer < fadeInTime)
        {
            fadeTimer += Time.deltaTime;
            float ratio = fadeTimer / fadeInTime;

            float alpha = Mathf.Lerp(0f, 1f, ratio);
            Color color = text.color;
            text.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        Color finalColor = text.color;
        text.color = new Color(finalColor.r, finalColor.g, finalColor.b, 1f);
    }
}
