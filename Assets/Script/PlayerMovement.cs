using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <플레이어 이동> 1번 생각  */
// 플레이어는 [walkable]큐브 위치에서 local 좌표 기준 (y?) +0.5 만큼에 위치해 있기
// 플레이어가 위치해 있는 큐브 묶음 [walkable]큐브 색상 변경
// walkable 큐브 묶음은 가중치 없는 방향 그래프?로 존재
// 갈찾기는 A*

/* <플레이어 이동> 2번 생각  */
// 플레이어 시작은 (tag. StartCube)큐브에서
// 플레이어는 큐브 위치에서 local 좌표 기준 (y?) +1.3 만큼에 위치해 있기
// 현재 플레이어가 있는 큐브의 라벨 알기
// 해당 라벨의 큐브들 받아오기 or 번호? 그래프로..
// 좌표(x, y, z) 받아오기 : 높이 y는 필요하지 않지 않을까..
// 2차원 배열에 넣기
// 현재 플레이어 위치에서 마우스 클릭한 위치까지 A*
// A*로 찾은 큐브길은 [walkable]큐브 색상 변경
// 플레이어는 해당 큐브길을 [walkable]큐브 좌표에 따라 Coroutine으로 시간 주면서 이동
// + 마우스 클릭 한 위치가 받아온 좌표상에 없으면 이동 안하기

/* <플레이어 이동> 3번 생각  */
// 플레이어 시작은 (tag. StartCube)큐브에서
// 플레이어는 [walkable]큐브 위치에서 local 좌표 기준 (y?) +0.8 만큼에 위치해 있기
// 현재 플레이어가 있는 큐브의 번호 & 라벨 찾기
// 클릭한 큐브 번호 & 라벨 찾기
// 두 라벨이 동일하지 않으면 플레이어 이동 안하기
// 현재 큐브 번호에서 클릭한 큐브 번호로 가는 길 cubeConnectionGraph 사용해서 Dijkstra / BFS로 길찾기
// 찾은 큐브 번호에 해당하는 GameObject 찾아서 저장하기
// 차례대로 GameObject의 transform 으로 회전하고 이동하고

public class PlayerMovement : MonoBehaviour
{
    // 
    private WalkablePath walkablePath;
    private GameObject[] connectWalkable;
    private int[,] cubeConnectionGraph;

    private bool checkOnce;

    // 시작 지점
    private GameObject startCube;
    private Vector3 startCubePos;

    // 플레이어
    private Vector3 playerCubePos;
    private int curPlayerCubeNum;
    private int curPlayerCubeLabel;

    // 클릭한 큐브
    private int curWalkableCubeNum;
    private int curWalkableCubeLabel;

    // BFS
    private Queue<int> pathFind = new Queue<int>();
    private bool[] visitedCube;
    private int[] pathCubeNum;
    private List<int> pathStore = new List<int>();

    // 이동
    private List<GameObject> pathCube = new List<GameObject>();
    private Vector3 targetPos;
    private Quaternion targetDir;
    private int index;
    private bool needMove;
    private bool needRotation;

    void Start()
    {
        // 플레이어 시작은 (tag. StartCube)큐브의 첫째 Child에서
        startCube = GameObject.FindGameObjectWithTag("StartCube").transform.GetChild(0).gameObject;
        // 플레이어는 [walkable]큐브 위치에서 local 좌표 기준 (y?) +0.8 만큼에 위치해 있기
        startCubePos = startCube.transform.position;
        transform.position = new Vector3(startCubePos.x, startCubePos.y + 0.8f, startCubePos.z);

        checkOnce = true;
    }

    void Update()
    {
        // 큐브 선택: 마우스 오른 클릭
        if (Input.GetMouseButtonDown(1))
        {
            // 처음 한번만 실행
            if (checkOnce)
            {
                // [walkable]큐브들 받아오기
                walkablePath = GameObject.FindObjectOfType<WalkablePath>();
                connectWalkable = walkablePath.connectWalkable;
                cubeConnectionGraph = walkablePath.cubeConnectionGraph;

                // 초기화
                visitedCube = new bool[connectWalkable.Length];
                pathCubeNum = new int[connectWalkable.Length];

                checkOnce = false;
            }

            // 클릭한 오브젝트
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                RaycastHit rayHit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
                {
                    // 클릭한 오브젝트가 [walkable]큐브 이면
                    if (rayHit.transform.gameObject.tag == "ConnectWalkable")
                    {
                        // [walkable]큐브면 해당 큐브 번호, 라벨 찾기
                        curWalkableCubeNum = rayHit.transform.gameObject.GetComponent<CubeState>().cubeNum;
                        curWalkableCubeLabel = rayHit.transform.gameObject.GetComponent<CubeState>().labelNum;
                        //Debug.Log("큐브 번호" + curWalkableCubeNum);
                        //Debug.Log("큐브 라벨" + curWalkableCubeLabel);

                        rayHit.transform.gameObject.GetComponent<Renderer>().material.color
                            = new Color(255 / 255f, 165 / 255f, 40 / 255f);   //주황

                        break;
                    }
                }
            }

            // 플레이어
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                // 현재 플레이어 위치와 [walkable]큐브 위치가 동일하면
                playerCubePos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
                if (playerCubePos == connectWalkable[i].transform.position)
                {
                    // 해당 [walkable]큐브의 번호, 라벨 알기
                    curPlayerCubeNum = connectWalkable[i].GetComponent<CubeState>().cubeNum;
                    curPlayerCubeLabel = connectWalkable[i].GetComponent<CubeState>().labelNum;
                    //Debug.Log("플레이어 번호 : " + curPlayerCubeNum);
                    //Debug.Log("플레이어 라벨 : " + curPlayerCubeLabel);

                    connectWalkable[i].transform.GetComponent<Renderer>().material.color
                        = new Color(255 / 255f, 165 / 255f, 40 / 255f);   //주황

                    break;
                }
            }

            // 플레이어와 클릭한 큐브가 동일한 라벨이면
            if (curWalkableCubeLabel == curPlayerCubeLabel)
            {
                // 초기화
                pathFind.Clear();
                for (int i = 0; i < connectWalkable.Length; i++)
                {
                    visitedCube[i] = false;
                    pathCubeNum[i] = -1;
                }

                // 현재 큐브 번호에서 클릭한 큐브 번호로 가는 길
                // 큐브 번호 BFS로 길찾기
                pathFind.Enqueue(curPlayerCubeNum);
                visitedCube[curPlayerCubeNum] = true;
                pathCubeNum[curPlayerCubeNum] = curPlayerCubeNum;

                while (pathFind.Count != 0)
                {
                    int curNum = pathFind.Dequeue();

                    for (int j = 0; j < connectWalkable.Length; j++)
                    {
                        // 인접하면서 방문전인 큐브
                        if (cubeConnectionGraph[curNum, j] == 1 && visitedCube[j] == false)
                        {
                            // 큐에 넣기
                            pathFind.Enqueue(j);
                            // 방문 갱신
                            visitedCube[j] = true;
                            // 부모 갱신
                            pathCubeNum[j] = curNum;
                        }
                    }

                    // 타겟 큐브를 방문 했으면 break
                    if (visitedCube[curWalkableCubeNum] == true)
                        break;
                }

                // 길 저장
                pathStore.Clear();
                pathStore.Add(curWalkableCubeNum);
                int temp = curWalkableCubeNum;
                while (temp != curPlayerCubeNum)
                {
                    pathStore.Add(pathCubeNum[temp]);
                    temp = pathCubeNum[temp];
                }

                // 뒤에서 부터 앞으로 읽기
                pathCube.Clear();
                for (int i = pathStore.Count - 1; i >= 0; i--)
                {
                    // 찾은 큐브 번호에 해당하는 GameObject 찾아서 저장하기
                    for (int j = 0; j < connectWalkable.Length; j++)
                    {
                        if (pathStore[i] == connectWalkable[j].GetComponent<CubeState>().cubeNum)
                        {
                            pathCube.Add(connectWalkable[j]);
                            break;
                        }
                    }
                }

                // 클릭한 위치 경로 구했으면 이동 준비 끝
                index = 0;
                needMove = true;
                needRotation = false;
            }
        }

        // 이동하자
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
