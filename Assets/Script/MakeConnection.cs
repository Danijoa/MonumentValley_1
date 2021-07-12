using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <���� �׷���> */
// trriger enter �� ���� (1) ���� �ٲ��ֱ� (2) [�θ�]ť�� ��ȣ �׷����� �������ֱ�
// trriger exit �� ���� (1) ���� �������� �����ֱ� (2) [�θ�]ť�� ��ȣ �׷������� ���� ����

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
            // (1) ���� �ٲ��ֱ�
            //isBlue = true;
            connectionCube.material.color = new Color(28 / 255f, 100 / 255f, 255 / 255f);

            // (2) ���� ��ȣ�� �ƴ϶�� / ť�� ��ȣ �׷����� �������ֱ�
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
            // (1) ���� �������� �����ֱ�
            //isBlue = false;
            connectionCube.material.color = startColor;

            // (2) ť�� ��ȣ �׷������� ���� ����
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