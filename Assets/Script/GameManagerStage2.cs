using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerStage2 : MonoBehaviour
{
    public static GameManagerStage2 instance;

    public bool isStairRotating;
    public bool isFloorRotating;

    private GameObject fadeImage;
    private SceneFade sceneFade;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        isStairRotating = false;
        isFloorRotating = false;

        fadeImage = GameObject.FindGameObjectWithTag("FadeUI");
        MakeFadeOut();
    }

    void Update()
    {
        
    }

    private void MakeFadeOut()
    {
        sceneFade = fadeImage.GetComponent<SceneFade>();
        sceneFade.FadeOut();
    }
}
