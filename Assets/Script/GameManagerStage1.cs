using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStage1 : MonoBehaviour
{
    private GameObject fadeImage;
    private SceneFade sceneFade;
    private bool once;
    void Start()
    {
        fadeImage = GameObject.FindGameObjectWithTag("FadeUI");
        once = true;
    }

    private void Update()
    {
        if (once)
        {
            MakeFadeOut();
            once = false;
        }
    }

    private void MakeFadeOut()
    {
        sceneFade = fadeImage.GetComponent<SceneFade>();
        sceneFade.FadeOut();
    }
}
