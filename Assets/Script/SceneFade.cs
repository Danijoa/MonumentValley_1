using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    private Image fadeImage;
    private Color fadeColor;
    private float fadeTime;
    private float startAlpha, endAlpha, time;
    private bool isFading;

	private void Start()
	{
        fadeImage = GetComponent<Image>();
        fadeColor = fadeImage.color;
        fadeTime = 2.5f;
        time = 0f;
        isFading = false;
    }

	public void FadeIn()
    {
        if (isFading)
            return;
        startAlpha = 0f;
        endAlpha = 1f;
        StartCoroutine("DoFadeIn");
    }

    IEnumerator DoFadeIn()
    {
        isFading = true;

        while (fadeColor.a < 1f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, time);
            fadeImage.color = fadeColor;
            yield return null;
        }

        SceneManager.LoadScene("Level2Scene");

        isFading = false;
    }

    public void FadeOut()
    {
        if (isFading)
            return;
        startAlpha = 1f;
        endAlpha = 0f;
        StartCoroutine("DoFadeOut");
    }

    IEnumerator DoFadeOut()
    {
        isFading = true;

        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, time);
            fadeImage.color = fadeColor;
            yield return null;
        }

        isFading = false;
    }
}
