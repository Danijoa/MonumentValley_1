using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int playerCubeNum;
    private GameObject playerRoad;
    public bool playerRoadIsFloor;
    private int curPlayerCubeNum;
    private int curPlayerCubeLabel;

    // 클릭한 큐브
    private int curWalkableCubeNum;
    private int curWalkableCubeLabel;
    private GameObject clickedCube;

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
    private Vector3 nextCubePos;
    private bool isIllusion;

    void Start()
    {
        // 플레이어 시작은 (tag. StartCube)큐브의 첫째 Child에서
        startCube = GameObject.FindGameObjectWithTag("StartCube").transform.GetChild(0).gameObject;
        // 플레이어는 [walkable]큐브 위치에서 local 좌표 기준 (y?) +0.8 만큼에 위치해 있기
        startCubePos = startCube.transform.position;
        PlayerCubePos(startCubePos);
        //transform.position = new Vector3(startCubePos.x, startCubePos.y + 0.8f, startCubePos.z);

        playerRoadIsFloor = false;
        isIllusion = false;

        clickedCube = GameObject.FindGameObjectWithTag("ClickPoint");
        clickedCube.SetActive(false);

        checkOnce = true;
    }

    void Update()
    {
        // 처음 한번만 실행
        if (checkOnce)
        {
            // [walkable]큐브들 받아오기
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            connectWalkable = walkablePath.connectWalkable;
            cubeConnectionGraph = walkablePath.cubeConnectionGraph;

            //walkablePath.MakePath();

            // 초기화
            visitedCube = new bool[connectWalkable.Length];
            pathCubeNum = new int[connectWalkable.Length];

            PlayerCube();

            checkOnce = false;
        }

        // 큐브 선택: 마우스 오른 클릭
        if (Input.GetMouseButtonDown(1))
        {
            // 클릭한 오브젝트
            FindClickedOdject();

            // 플레이어
            FindPlayer();

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
                PathBFS();

                // 길 저장
                StorePath();

                // 클릭한 위치 경로 구했으면 이동 준비 끝
                index = 0;
                if (pathCube.Count >= 2)
                {
                    needMove = false;
                    needRotation = true;
                }
                //Debug.Log("길 : " + pathCube.Count);
            }
        }

        // 회전하자
        if (needRotation)
        {
            // 이동할 목표 지점 큐브가 착시 포인트(Illusion1Up)인가 확인
            if (pathCube[index].transform.parent.tag == "Illusion1Up" && pathCube[index + 1].transform.parent.tag == "Illusion1Down"
                || pathCube[index].transform.parent.tag == "Illusion1Down" && pathCube[index + 1].transform.parent.tag == "Illusion1Up")
                isIllusion = true;

            // 바라보는 방향
            Vector3 dir;
            if (isIllusion)
            {
                targetDir = transform.rotation;
            }
            else
            {
                dir = new Vector3(pathCube[index + 1].transform.position.x, 0, pathCube[index + 1].transform.position.z)
                    - new Vector3(pathCube[index].transform.position.x, 0, pathCube[index].transform.position.z);
                targetDir = Quaternion.LookRotation(dir);
            }

            // 회전 적용
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetDir, Time.deltaTime * 400f);

            // 회전 완료
            if (transform.rotation == targetDir)
            {
                needRotation = false;
                needMove = true;
            }
        }

        // 이동하자
        if (needMove && !needRotation)
        {
            // 이동 위치 잡기
            nextCubePos = pathCube[index + 1].transform.position;
            targetPos = new Vector3(nextCubePos.x, nextCubePos.y + 0.8f, nextCubePos.z);

            // 이동 적용
            if (isIllusion)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 50f);
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 8f);

            // 이동 완료 
            if (transform.position == targetPos)
            {
                isIllusion = false;

                // 이동한 위치의 플레이어 큐브 번호 찾기
                PlayerCube();

                // 다음 이동 큐브 
                if (index + 1 >= pathCube.Count - 1)
                {
                    needMove = false;
                    needRotation = false;
                }
                else
                {
                    index++;
                    needMove = false;
                    needRotation = true;
                }
            }
        }
    }
    private void FindClickedOdject()
    {
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

                    StartCoroutine(DrawCircle(rayHit.transform.gameObject.transform.position));

                    rayHit.transform.gameObject.GetComponent<Renderer>().material.color
                        = new Color(255 / 255f, 165 / 255f, 40 / 255f);   //주황

                    break;
                }
            }
        }
    }

    IEnumerator DrawCircle(Vector3 cubePos)
    {
        // 위치 설정
        clickedCube.transform.position = new Vector3(cubePos.x, cubePos.y + 0.02f, cubePos.z);
        // 활성화
        clickedCube.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        // 비활성화
        StartCoroutine("DoFadeOut");
    }
    
    IEnumerator DoFadeOut()
    {
        float time = 0f;
        Material fadeMaterial = clickedCube.GetComponent<Renderer>().material;
        Color fadeColor = fadeMaterial.color;
        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / 1f;
            fadeColor.a = Mathf.Lerp(1f, 0f, time);
            fadeMaterial.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1f;
        fadeMaterial.color = fadeColor;
        clickedCube.SetActive(false);
    }

    private void FindPlayer()
    {
        for (int i = 0; i < connectWalkable.Length; i++)
        {
            // 현재 플레이어 위치와 [walkable]큐브 위치가 동일하면
            playerCubePos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
            Vector3 temp = playerCubePos - connectWalkable[i].transform.position;

            if (temp.magnitude < 0.1f)
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
    }

    private void PathBFS()
    {
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
    }

    private void StorePath()
    {
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
    }

    private void PlayerCube()
    {
        for (int i = 0; i < connectWalkable.Length; i++)
        {
            Vector3 tempPos1 = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
            if (tempPos1 == connectWalkable[i].transform.position)
            {
                playerCubeNum = connectWalkable[i].GetComponent<CubeState>().cubeNum;
                //Debug.Log("번호: " + playerCubeNum);
                break;
            }
        }

        for (int i = 0; i < connectWalkable.Length; i++)
        {
            Vector3 tempPos2 = new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z);
            if (tempPos2 == connectWalkable[i].transform.parent.transform.position)
            {
                playerRoad = connectWalkable[i].transform.parent.gameObject;

                if (playerRoad.tag == "FloorCube" || playerRoad.tag == "Illusion1Down")
                    playerRoadIsFloor = true;
                else
                    playerRoadIsFloor = false;

                break;
            }
        }
    }

    public void PlayerCubePos(Vector3 playerCube)
    {
        transform.position = new Vector3(playerCube.x, playerCube.y + 0.8f, playerCube.z);
    }
}