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
    private float fadeInTime; 
    private float startAlpha, endAlpha, time;
    private bool isFading;

    private Scene scene;

    private void Start()
	{
        fadeImage = GetComponent<Image>();
        fadeColor = fadeImage.color;
        fadeTime = 4f;
        fadeInTime = 2f;
        time = 0f;
        isFading = false;

        scene = SceneManager.GetActiveScene();
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

        time = 0;
        while (fadeColor.a < 1f)
        {
            time += Time.deltaTime / fadeInTime;
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, time);
            fadeImage.color = fadeColor;
            yield return null;
        }

        if (scene.name == "Level1Scene")
        {
            SceneManager.LoadScene("Level2Scene");
        }

        if (scene.name == "StartScene")
        {
            SceneManager.LoadScene("Level1Scene");
        }


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

        time = 0;
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
