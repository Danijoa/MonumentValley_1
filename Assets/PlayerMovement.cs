using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <�÷��̾� �̵�> 1�� ����  */
// �÷��̾�� [walkable]ť�� ��ġ���� local ��ǥ ���� (y?) +0.5 ��ŭ�� ��ġ�� �ֱ�
// �÷��̾ ��ġ�� �ִ� ť�� ���� [walkable]ť�� ���� ����
// walkable ť�� ������ ����ġ ���� ���� �׷���?�� ����
// ��ã��� a*

/* <�÷��̾� �̵�> 2�� ����  */
// �÷��̾� ������ (tag. StartCube)ť�꿡��
// �÷��̾�� ť�� ��ġ���� local ��ǥ ���� (y?) +1.3 ��ŭ�� ��ġ�� �ֱ�
// ���� �÷��̾ �ִ� ť���� �� �˱�
// �ش� ���� ť��� �޾ƿ��� or ��ȣ? �׷�����..
// ��ǥ(x, y, z) �޾ƿ��� : ���� y�� �ʿ����� ���� ������..
// 2���� �迭�� �ֱ�
// ���� �÷��̾� ��ġ���� ���콺 Ŭ���� ��ġ���� A*
// A*�� ã�� ť����� [walkable]ť�� ���� ����
// �÷��̾�� �ش� ť����� [walkable]ť�� ��ǥ�� ���� Coroutine���� �ð� �ָ鼭 �̵�
// + ���콺 Ŭ�� �� ��ġ�� �޾ƿ� ��ǥ�� ������ �̵� ���ϱ�

/* <�÷��̾� �̵�> 2�� ����  */
// �÷��̾� ������ (tag. StartCube)ť�꿡��
// �÷��̾�� ť�� ��ġ���� local ��ǥ ���� (y?) +1.3 ��ŭ�� ��ġ�� �ֱ�
// ���� �÷��̾ �ִ� ť���� ��ȣ & �� ã��

// Ŭ���� ť�� ��ȣ & �� ã��

// �� ���� �������� ������ �÷��̾� �̵� ���ϱ�

// ���� ť�� ��ȣ���� Ŭ���� ť�� ��ȣ�� ���� �� cubeConnectionGraph ����ؼ� Dijkstra�� ��ã��

// ã�� �� ���� ������ �ֱ�

// �÷��̾�� �ش� ť����� [walkable]ť�� ��ǥ�� ���� Coroutine���� �ð� �ָ鼭 �̵�


public class PlayerMovement : MonoBehaviour
{
    private GameObject startCube;
    private Vector3 startCubePos;

    private WalkablePath walkablePath;
    private GameObject[] connectWalkable;
    private int[,] cubeConnectionGraph;

    private int curPlayerCubeNum;
    private int curPlayerCubeLabel;

    private bool checkOnce;

    void Start()
    {
        // �÷��̾� ������ (tag. StartCube)ť�꿡��
        startCube = GameObject.FindGameObjectWithTag("StartCube");
        // �÷��̾�� ť�� ��ġ���� local ��ǥ ���� (y?) +1.3 ��ŭ�� ��ġ�� �ֱ�
        startCubePos = startCube.transform.position;
        transform.position = new Vector3(startCubePos.x, startCubePos.y + 1.3f, startCubePos.z);

        checkOnce = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ó�� �ѹ��� ����
            if (checkOnce)
            {
                // [walkable]ť��� �޾ƿ���
                walkablePath = GameObject.FindObjectOfType<WalkablePath>();
                connectWalkable = walkablePath.connectWalkable;
                cubeConnectionGraph = walkablePath.cubeConnectionGraph;

                checkOnce = false;
            }

            // ��ȣ�� �� ã��
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                // ���� �÷��̾� ��ġ�� (�θ�)ť�� ��ġ�� �����ϸ�
                if (startCubePos == connectWalkable[i].transform.position)
                {
                    // �ش� ť���� ��ȣ, �� �˱�
                    curPlayerCubeNum = connectWalkable[i].GetComponent<CubeState>().cubeNum;
                    curPlayerCubeLabel = connectWalkable[i].GetComponent<CubeState>().labelNum;
                    Debug.Log("curPlayerCubeNum : " + curPlayerCubeNum);
                    connectWalkable[i].GetComponent<Renderer>().material.color
                        = new Color(255 / 255f, 0 / 255f, 255 / 255f);   //����
				}
			}
        }
	}
}
