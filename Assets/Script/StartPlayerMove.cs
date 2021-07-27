using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayerMove : MonoBehaviour
{
    private GameObject eve;
    private float time;
    public Material[] materials;
    private Renderer rend;

    private GameObject buttonObject;
    private MakeButton nextStageButton;

    void Start()
    {
        eve = GameObject.FindGameObjectWithTag("EveFace");
        rend = eve.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];

        StartCoroutine(MakeMovement());   
    }

    IEnumerator MakeMovement()
    {
        // 화면 중앙
        time = 0f;
        Vector3 toScreen = new Vector3(0, -1.1f, 18.4f);
        while (transform.position.x  > 0.1f)
        { 
            time += Time.deltaTime / 10f;
            transform.position = Vector3.Lerp(transform.position, toScreen, time);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);

        // 화면 앞으로
        time = 0f;
        Vector3 toFront = new Vector3(0, -1.1f, 2.28f);
        while (transform.position.z > 2.29f)
        {
            time += Time.deltaTime / 8f;
            transform.position = Vector3.Lerp(transform.position, toFront, time);
            yield return null;
        }

        // 고개 좌우
        time = 0f;
        float timeCnt = 0f;
        Quaternion toLR = Quaternion.Euler(0, 180, -1.3f);
        while (timeCnt <= 1.5f)
        {
            timeCnt += Time.deltaTime;
            time += Time.deltaTime / 24f;
            transform.rotation = Quaternion.Lerp(transform.rotation, toLR, time);
            if (transform.rotation == Quaternion.Euler(0, 180, -1.3f))
            {
                toLR = Quaternion.Euler(0, 180, 1.3f);
                time = 0;
                yield return new WaitForSeconds(0.11f);
            }
            else if (transform.rotation == Quaternion.Euler(0, 180, 1.3f))
            {
                toLR = Quaternion.Euler(0, 180, -1.3f);
                time = 0;
                yield return new WaitForSeconds(0.11f);
            }
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 180, 0);
        // 표정 바뀌기
        rend.sharedMaterial = materials[1];

        // 화면 뒤로
        time = 0f;
        Vector3 toBack = new Vector3(0, -1.13f, 2.75f);
        while (transform.position.z < 2.74f)
        {
            time += Time.deltaTime / 8f;
            transform.position = Vector3.Lerp(transform.position, toBack, time);
            yield return null;
        }

        // 표정 바뀌기
        yield return new WaitForSeconds(0.2f);
        rend.sharedMaterial = materials[2];
        // 고개 상하
        time = 0f;
        timeCnt = 0f;
        int i = -1;
        Vector3 toUD = toBack;
        while (timeCnt <= 1.5f)
        {
            timeCnt += Time.deltaTime;
            time += Time.deltaTime / 16f;
            transform.position = Vector3.Lerp(transform.position, toUD, time);
            Vector3 temp = transform.position - toUD;
            if (temp.magnitude < 0.009f)
            {
                toUD = new Vector3(toBack.x, toBack.y + 0.02f * i, toBack.z);
                time = 0;
            }
            i *= -1;
            yield return null;
        }
        transform.position = toBack;

        // 표정 바뀌기
        yield return new WaitForSeconds(0.2f);
        rend.sharedMaterial = materials[1];

        // 버튼
        buttonObject = GameObject.FindGameObjectWithTag("NextButton");
        nextStageButton = buttonObject.GetComponent<MakeButton>();
        nextStageButton.ButtonAppear();

        // 위아래
        while (true)
        {
            time += Time.deltaTime / 16f;
            transform.position = Vector3.Lerp(transform.position, toUD, time);
            Vector3 temp = transform.position - toUD;
            if (temp.magnitude < 0.009f)
            {
                toUD = new Vector3(toBack.x, toBack.y + 0.01f * i, toBack.z);
                time = 0;
            }
            i *= -1;
            yield return null;
        }
    }
}
