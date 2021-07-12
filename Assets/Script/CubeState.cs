using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    public static int cubeNumber = 0;

    public int cubeNum;     // 큐브 고유 번호
    public int labelNum;    // 라벨링되어 있는 번호
    public bool walkable;   // 플레이어가 이동 가능한 큐브인가

    Renderer colorChange;

    private void Awake()
    {
        cubeNum = cubeNumber++;
        //Debug.Log("cubeNum : " + cubeNum);
    }

    private void Start()
    {
        labelNum = -1;
        walkable = false;

        colorChange = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        // 색상 구분
        if (labelNum == 1)
            colorChange.material.color = new Color(0 / 255f, 255 / 255f, 255 / 255f);   //하늘
        else if (labelNum == 2)
            colorChange.material.color = new Color(255 / 255f, 0 / 255f, 255 / 255f);   //보라
        else if (labelNum == 3)
            colorChange.material.color = new Color(255 / 255f, 0 / 255f, 0 / 255f);     //빨간
        else if (labelNum == 4)
            colorChange.material.color = new Color(0 / 255f, 0 / 255f, 255 / 255f);     //파랑
        else if (labelNum == 5)
            colorChange.material.color = new Color(0 / 255f, 255 / 255f, 0 / 255f);     //초록
        else if (labelNum == 6)
            colorChange.material.color = new Color(0 / 255f, 0 / 255f, 0 / 255f);       //검정
        else
            colorChange.material.color = new Color(255 / 255f, 255 / 255f, 0 / 255f);   //노랑
    }
}
