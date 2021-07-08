using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeState : MonoBehaviour
{
    public static int cubeNumber = 0;

    public int cubeNum;     // 큐브 고유 번호
    public int labelNum;    // 라벨링되어 있는 번호
    public bool walkable;   // 플레이어가 이동 가능한 큐브인가

    public MakeConnection[] child;  // 자식 큐브 [up down left right] 
    public bool[] childBlue;        // 연결되어있는지 확인

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

        child = new MakeConnection[4];
        childBlue = new bool[4];

        child[0] = transform.GetChild(0).GetComponent<MakeConnection>();
        child[1] = transform.GetChild(1).GetComponent<MakeConnection>();
        child[2] = transform.GetChild(2).GetComponent<MakeConnection>();
        child[3] = transform.GetChild(3).GetComponent<MakeConnection>();

        colorChange = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        // 0:up 1:down 2:left 3:right
        childBlue[0] = child[0].isBlue;
        childBlue[1] = child[1].isBlue;
        childBlue[2] = child[2].isBlue;
        childBlue[3] = child[3].isBlue;

        // 색상 구분
        if (gameObject.tag != "CheckStair")
        { 
            if (labelNum == 1)
                colorChange.material.color = new Color(0 / 255f, 255 / 255f, 255 / 255f);
            else if (labelNum == 2)
                colorChange.material.color = new Color(255 / 255f, 255 / 255f, 0 / 255f);
            else if (labelNum == 3)
                colorChange.material.color = new Color(255 / 255f, 0 / 255f, 0 / 255f);
            else if (labelNum == 4)
                colorChange.material.color = new Color(0 / 255f, 0 / 255f, 255 / 255f);
            else if (labelNum == 5)
                colorChange.material.color = new Color(0 / 255f, 255 / 255f, 0 / 255f);
            else if (labelNum == 6)
                colorChange.material.color = new Color(255 / 255f, 0 / 255f, 255 / 255f);
            else if (labelNum == 7)
                colorChange.material.color = new Color(0 / 255f, 0 / 255f, 0 / 255f);
        }
    }
}
