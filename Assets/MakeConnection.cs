using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// trriger enter 된 순간 (1) 색상 바꿔주기 (2) [부모]큐브 번호 그래프에 연결해주기
// trriger exit 된 순간 (1) 원래 색상으로 돌려주기 (2) [부모]큐브 번호 그래프에서 연결 해제

public class MakeConnection : MonoBehaviour
{
    public bool isBlue;

    Renderer connectionCube;
    Color startColor;

    int curCubeNum;
    int otherCubeNum;

    WalkablePath walkablePath;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ConnectionIndicator")
        {
            // (1) 색상 바꿔주기
            isBlue = true;
            connectionCube.material.color = new Color(28 / 255f, 100 / 255f, 255 / 255f);

            // (2) 큐브 번호 그래프에 연결해주기
            otherCubeNum = other.GetComponentInParent<CubeState>().cubeNum;
            walkablePath.cubeConnectionGraph[curCubeNum, otherCubeNum] = 1;
            walkablePath.cubeConnectionGraph[otherCubeNum, curCubeNum] = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ConnectionIndicator")
        {
            // (1) 원래 색상으로 돌려주기
            isBlue = false;
            connectionCube.material.color = startColor;

            // (2) 큐브 번호 그래프에서 연결 해제
            walkablePath.cubeConnectionGraph[curCubeNum, otherCubeNum] = 0;
            walkablePath.cubeConnectionGraph[otherCubeNum, curCubeNum] = 0;
        }

    }

    void Start()
    {
        isBlue = false;
        connectionCube = gameObject.GetComponent<Renderer>();
        startColor = connectionCube.material.color;

        curCubeNum = gameObject.GetComponentInParent<CubeState>().cubeNum;
        otherCubeNum = 0;

        walkablePath = GameObject.FindObjectOfType<WalkablePath>();
    }

    void Update()
    {
    }
}