using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/* <�� ����> 1�� ���� */
// [walkable]ť��(tag. ConnectWalkable)�� �����ϴ� ��� [�θ�]ť�� �޾ƿ���
// [�θ�]ť�긦 �ϳ��ϳ� Ȯ���ϸ鼭 �Ʒ� �ִ� [up down left right]ť�� �Ķ��� �����ִ��� Ȯ��
// �Ķ��� �ִ� ť�곢�� ���� -> �󺧸� ó��..

/* <�� ����> 2�� ���� */
// [walkable]ť��(tag. ConnectWalkable)�� �����ϴ� ��� [�θ�]ť�� �޾ƿ���
// 1�� ť�� ���� ���� �׷��� Ȯ���ϸ鼭 ���� ť�� �޾ƿ���
// bfs �󺧸�

/* <���� ����> */
// �̶� ���ÿ� �ʿ��� ť����� ������ �����Ű��

public class WalkablePath : MonoBehaviour
{
    public int[,] cubeConnectionGraph;

    public GameObject[] connectWalkable;    
    public CubeState[] cubeState;          
    private int walkableCubeNum;

    private CubeState curCube;
    private CubeState nextCube;
    private Queue<CubeState> myQ;
    private int label;
    private int curCubeNum;

    void Start()
    {
        // [walkable]ť��(tag. ConnectWalkable)�� �����ϴ� ť�� �޾ƿ���
        connectWalkable = GameObject.FindGameObjectsWithTag("ConnectWalkable");
        walkableCubeNum = connectWalkable.Length;

        // ť�� ��ȣ ���� �׷��� ���� �迭 ���� : ��ü 0 �ʱ�ȭ �Ǿ��ִ� (0: �񿬰� / 1: ����)
        cubeConnectionGraph = new int[walkableCubeNum, walkableCubeNum];

        // CubeState ��ũ��Ʈ ��������
        cubeState = new CubeState[walkableCubeNum];
        for (int i = 0; i < walkableCubeNum; i++)
        {
            cubeState[i] = connectWalkable[i].GetComponent<CubeState>();
        }

        // ť ����
        myQ = new Queue<CubeState>();

        // ���� ��
        label = 1;
    }

    public void MakePath()
    {
        for (int i = 0; i < walkableCubeNum; i++)
            cubeState[i].labelNum = -1;
        label = 1;

        for (int i = 0; i < walkableCubeNum; i++)
        {
            // �̹� Ȯ�ε� ť�� 
            if (cubeState[i].labelNum != -1)
                continue;

            // ť�� ť��(CubeState) �ֱ�
            myQ.Enqueue(cubeState[i]);

            while (myQ.Count != 0)
            {
                // ���� ť�� ť���� ť�� ���� ��ȯ
                curCube = myQ.Dequeue();
                // ���� ť�� �� �����ϱ�
                curCube.labelNum = label;
                // ���� ť�� ��ȣ �޾ƿ���
                curCubeNum = curCube.cubeNum;

                // ���� ť��� ����Ǿ� �ִ� ť�� ��ȣ�� �׷������� Ȯ���ϱ�
                // ��ȣ�� �ش��ϴ� ť��(CubeState)�� ���� �󺧸� ���� �ʾҴٸ�
                // �ϳ��� �������ͼ� �� ������ �ְ� ť�� ť�� �ֱ�
                for (int j = 0; j < walkableCubeNum; j++)
                {
                    if (cubeConnectionGraph[curCubeNum, j] == 1)
                    {
                        nextCube = null;

                        for (int k = 0; k < walkableCubeNum; k++)
                        {
                            if (cubeState[k].cubeNum == j && cubeState[k].labelNum == -1)
                            {
                                nextCube = cubeState[k];
                                break;
                            }
                        }

                        if (nextCube != null)
                        {
                            nextCube.labelNum = label;
                            myQ.Enqueue(nextCube);
                        }
                    }
                }
            }

            // ���� �� �׷����� �Ѿ��
            label++;
        }
    }
}
