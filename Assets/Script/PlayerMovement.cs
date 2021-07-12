using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <�÷��̾� �̵�> 1�� ����  */
// �÷��̾�� [walkable]ť�� ��ġ���� local ��ǥ ���� (y?) +0.5 ��ŭ�� ��ġ�� �ֱ�
// �÷��̾ ��ġ�� �ִ� ť�� ���� [walkable]ť�� ���� ����
// walkable ť�� ������ ����ġ ���� ���� �׷���?�� ����
// ��ã��� A*

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

/* <�÷��̾� �̵�> 3�� ����  */
// �÷��̾� ������ (tag. StartCube)ť�꿡��
// �÷��̾�� [walkable]ť�� ��ġ���� local ��ǥ ���� (y?) +0.8 ��ŭ�� ��ġ�� �ֱ�
// ���� �÷��̾ �ִ� ť���� ��ȣ & �� ã��
// Ŭ���� ť�� ��ȣ & �� ã��
// �� ���� �������� ������ �÷��̾� �̵� ���ϱ�
// ���� ť�� ��ȣ���� Ŭ���� ť�� ��ȣ�� ���� �� cubeConnectionGraph ����ؼ� Dijkstra / BFS�� ��ã��
// ã�� ť�� ��ȣ�� �ش��ϴ� GameObject ã�Ƽ� �����ϱ�
// ���ʴ�� GameObject�� transform ���� ȸ���ϰ� �̵��ϰ�

public class PlayerMovement : MonoBehaviour
{
    // 
    private WalkablePath walkablePath;
    private GameObject[] connectWalkable;
    private int[,] cubeConnectionGraph;

    private bool checkOnce;

    // ���� ����
    private GameObject startCube;
    private Vector3 startCubePos;

    // �÷��̾�
    private Vector3 playerCubePos;
    private int curPlayerCubeNum;
    private int curPlayerCubeLabel;

    // Ŭ���� ť��
    private int curWalkableCubeNum;
    private int curWalkableCubeLabel;

    // BFS
    private Queue<int> pathFind = new Queue<int>();
    private bool[] visitedCube;
    private int[] pathCubeNum;
    private List<int> pathStore = new List<int>();

    // �̵�
    private List<GameObject> pathCube = new List<GameObject>();
    private Vector3 targetPos;
    private Quaternion targetDir;
    private int index;
    private bool needMove;
    private bool needRotation;

    void Start()
    {
        // �÷��̾� ������ (tag. StartCube)ť���� ù° Child����
        startCube = GameObject.FindGameObjectWithTag("StartCube").transform.GetChild(0).gameObject;
        // �÷��̾�� [walkable]ť�� ��ġ���� local ��ǥ ���� (y?) +0.8 ��ŭ�� ��ġ�� �ֱ�
        startCubePos = startCube.transform.position;
        transform.position = new Vector3(startCubePos.x, startCubePos.y + 0.8f, startCubePos.z);

        checkOnce = true;
    }

    void Update()
    {
        // ť�� ����: ���콺 ���� Ŭ��
        if (Input.GetMouseButtonDown(1))
        {
            // ó�� �ѹ��� ����
            if (checkOnce)
            {
                // [walkable]ť��� �޾ƿ���
                walkablePath = GameObject.FindObjectOfType<WalkablePath>();
                connectWalkable = walkablePath.connectWalkable;
                cubeConnectionGraph = walkablePath.cubeConnectionGraph;

                // �ʱ�ȭ
                visitedCube = new bool[connectWalkable.Length];
                pathCubeNum = new int[connectWalkable.Length];

                checkOnce = false;
            }

            // Ŭ���� ������Ʈ
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                RaycastHit rayHit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
                {
                    // Ŭ���� ������Ʈ�� [walkable]ť�� �̸�
                    if (rayHit.transform.gameObject.tag == "ConnectWalkable")
                    {
                        // [walkable]ť��� �ش� ť�� ��ȣ, �� ã��
                        curWalkableCubeNum = rayHit.transform.gameObject.GetComponent<CubeState>().cubeNum;
                        curWalkableCubeLabel = rayHit.transform.gameObject.GetComponent<CubeState>().labelNum;
                        //Debug.Log("ť�� ��ȣ" + curWalkableCubeNum);
                        //Debug.Log("ť�� ��" + curWalkableCubeLabel);

                        rayHit.transform.gameObject.GetComponent<Renderer>().material.color
                            = new Color(255 / 255f, 165 / 255f, 40 / 255f);   //��Ȳ

                        break;
                    }
                }
            }

            // �÷��̾�
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                // ���� �÷��̾� ��ġ�� [walkable]ť�� ��ġ�� �����ϸ�
                playerCubePos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
                if (playerCubePos == connectWalkable[i].transform.position)
                {
                    // �ش� [walkable]ť���� ��ȣ, �� �˱�
                    curPlayerCubeNum = connectWalkable[i].GetComponent<CubeState>().cubeNum;
                    curPlayerCubeLabel = connectWalkable[i].GetComponent<CubeState>().labelNum;
                    //Debug.Log("�÷��̾� ��ȣ : " + curPlayerCubeNum);
                    //Debug.Log("�÷��̾� �� : " + curPlayerCubeLabel);

                    connectWalkable[i].transform.GetComponent<Renderer>().material.color
                        = new Color(255 / 255f, 165 / 255f, 40 / 255f);   //��Ȳ

                    break;
                }
            }

            // �÷��̾�� Ŭ���� ť�갡 ������ ���̸�
            if (curWalkableCubeLabel == curPlayerCubeLabel)
            {
                // �ʱ�ȭ
                pathFind.Clear();
                for (int i = 0; i < connectWalkable.Length; i++)
                {
                    visitedCube[i] = false;
                    pathCubeNum[i] = -1;
                }

                // ���� ť�� ��ȣ���� Ŭ���� ť�� ��ȣ�� ���� ��
                // ť�� ��ȣ BFS�� ��ã��
                pathFind.Enqueue(curPlayerCubeNum);
                visitedCube[curPlayerCubeNum] = true;
                pathCubeNum[curPlayerCubeNum] = curPlayerCubeNum;

                while (pathFind.Count != 0)
                {
                    int curNum = pathFind.Dequeue();

                    for (int j = 0; j < connectWalkable.Length; j++)
                    {
                        // �����ϸ鼭 �湮���� ť��
                        if (cubeConnectionGraph[curNum, j] == 1 && visitedCube[j] == false)
                        {
                            // ť�� �ֱ�
                            pathFind.Enqueue(j);
                            // �湮 ����
                            visitedCube[j] = true;
                            // �θ� ����
                            pathCubeNum[j] = curNum;
                        }
                    }

                    // Ÿ�� ť�긦 �湮 ������ break
                    if (visitedCube[curWalkableCubeNum] == true)
                        break;
                }

                // �� ����
                pathStore.Clear();
                pathStore.Add(curWalkableCubeNum);
                int temp = curWalkableCubeNum;
                while (temp != curPlayerCubeNum)
                {
                    pathStore.Add(pathCubeNum[temp]);
                    temp = pathCubeNum[temp];
                }

                // �ڿ��� ���� ������ �б�
                pathCube.Clear();
                for (int i = pathStore.Count - 1; i >= 0; i--)
                {
                    // ã�� ť�� ��ȣ�� �ش��ϴ� GameObject ã�Ƽ� �����ϱ�
                    for (int j = 0; j < connectWalkable.Length; j++)
                    {
                        if (pathStore[i] == connectWalkable[j].GetComponent<CubeState>().cubeNum)
                        {
                            pathCube.Add(connectWalkable[j]);
                            break;
                        }
                    }
                }

                // Ŭ���� ��ġ ��� �������� �̵� �غ� ��
                index = 0;
                needMove = true;
                needRotation = false;
            }
        }

        // �̵�����
        if (needMove && !needRotation)
        {
            Vector3 nextCubePos = pathCube[index].transform.position;
            targetPos = new Vector3(nextCubePos.x, nextCubePos.y + 0.8f, nextCubePos.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 8f);

            if (transform.position == targetPos)
            {
                index++;
                needRotation = true;
            }

            if (index >= pathCube.Count)
            {
                needMove = false;
                needRotation = false;
            }
        }

        if (needRotation)
        {
            //Vector3 dir = pathCube[index].transform.position - new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z); ;
            Vector3 dir = new Vector3(pathCube[index].transform.position.x, 0, pathCube[index].transform.position.z) 
                - new Vector3(transform.position.x, 0, transform.position.z); ;
            targetDir = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetDir, Time.deltaTime * 200f);
            if (transform.rotation == targetDir) 
            {
                needRotation = false;
            }
        }
	}
}
