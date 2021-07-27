using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour
{
    private RawImage buttonImage;
    private Color buttonColor;

    private float buttonTime;
    private float startAlpha, endAlpha, time;
    private bool isButton;
    bool isClicked;

    private GameObject fadeImage;
    private SceneFade sceneFade;

    private void Start()
    {
        buttonImage = GetComponent<RawImage>();
        buttonColor = buttonImage.color;
        buttonTime = 0.8f;
        time = 0f;
        isButton = false;
        isClicked = false;

        fadeImage = GameObject.FindGameObjectWithTag("FadeUI");
    }

    public void ButtonAppear()
    {
        if (isButton)
            return;

        startAlpha = 0f;// 0.31f;  // 80 / 255
        endAlpha = 1f;// 0.58f;   // 150 / 255

        StartCoroutine("DoButtonAppear");
    }

    IEnumerator DoButtonAppear()
    {
        isButton = true;
        bool appear = true;

        while (true)
        {
            if (isClicked) yield break;

            time += Time.deltaTime / buttonTime;
            if (appear)
            {
                buttonColor.a = Mathf.Lerp(startAlpha, endAlpha, time);
                if (buttonColor.a >= endAlpha)
                {
                    appear = false;
                    time = 0f;
                }
            }
            else
            {
                buttonColor.a = Mathf.Lerp(endAlpha, startAlpha, time);
                if (buttonColor.a <= startAlpha)
                {
                    appear = true;
                    time = 0f;
                }
            }

            buttonImage.color = buttonColor;

            yield return null;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isClicked = true;
            CubeState.cubeNumber = 0;
            sceneFade = fadeImage.GetComponent<SceneFade>();

            sceneFade.FadeIn();
        }
    }

    public void OnClickTest()
    {
        Debug.Log("??");
    }
}
