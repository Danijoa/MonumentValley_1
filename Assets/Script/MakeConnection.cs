using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <연결 그래프> */
// trriger enter 된 순간 (1) 색상 바꿔주기 (2) [부모]큐브 번호 그래프에 연결해주기
// trriger exit 된 순간 (1) 원래 색상으로 돌려주기 (2) [부모]큐브 번호 그래프에서 연결 해제

public class MakeConnection : MonoBehaviour
{
    private Renderer connectionCube;
    private Color startColor;

    private int curCubeNum;
    private int otherCubeNum;

    private WalkablePath walkablePath;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ConnectionIndicator")
        {
            // (1) 색상 바꿔주기
            //isBlue = true;
            connectionCube.material.color = new Color(28 / 255f, 100 / 255f, 255 / 255f);

            // (2) 같은 번호가 아니라면 / 큐브 번호 그래프에 연결해주기
            otherCubeNum = other.GetComponentInParent<CubeState>().cubeNum;
            if (other.GetComponentInParent<RoadState>().roadNum 
                != this.GetComponentInParent<RoadState>().roadNum)
            {
                walkablePath.cubeConnectionGraph[curCubeNum, otherCubeNum] = 1;
                walkablePath.cubeConnectionGraph[otherCubeNum, curCubeNum] = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ConnectionIndicator")
        {
            // (1) 원래 색상으로 돌려주기
            //isBlue = false;
            connectionCube.material.color = startColor;

            // (2) 큐브 번호 그래프에서 연결 해제
            walkablePath.cubeConnectionGraph[curCubeNum, otherCubeNum] = 0;
            walkablePath.cubeConnectionGraph[otherCubeNum, curCubeNum] = 0;
        }

    }

    void Start()
    {
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