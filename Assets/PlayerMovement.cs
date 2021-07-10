using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <플레이어 이동> 1번 생각  */
// 플레이어는 [walkable]큐브 위치에서 local 좌표 기준 (y?) +0.5 만큼에 위치해 있기
// 플레이어가 위치해 있는 큐브 묶음 [walkable]큐브 색상 변경
// walkable 큐브 묶음은 가중치 없는 방향 그래프?로 존재
// 갈찾기는 a*

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

/* <플레이어 이동> 2번 생각  */
// 플레이어 시작은 (tag. StartCube)큐브에서
// 플레이어는 큐브 위치에서 local 좌표 기준 (y?) +1.3 만큼에 위치해 있기
// 현재 플레이어가 있는 큐브의 번호 & 라벨 찾기

// 클릭한 큐브 번호 & 라벨 찾기

// 두 라벨이 동일하지 않으면 플레이어 이동 안하기

// 현재 큐브 번호에서 클릭한 큐브 번호로 가는 길 cubeConnectionGraph 사용해서 Dijkstra로 길찾기

// 찾은 길 색상 변경해 주기

// 플레이어는 해당 큐브길을 [walkable]큐브 좌표에 따라 Coroutine으로 시간 주면서 이동


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
        // 플레이어 시작은 (tag. StartCube)큐브에서
        startCube = GameObject.FindGameObjectWithTag("StartCube");
        // 플레이어는 큐브 위치에서 local 좌표 기준 (y?) +1.3 만큼에 위치해 있기
        startCubePos = startCube.transform.position;
        transform.position = new Vector3(startCubePos.x, startCubePos.y + 1.3f, startCubePos.z);

        checkOnce = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // 처음 한번만 실행
            if (checkOnce)
            {
                // [walkable]큐브들 받아오기
                walkablePath = GameObject.FindObjectOfType<WalkablePath>();
                connectWalkable = walkablePath.connectWalkable;
                cubeConnectionGraph = walkablePath.cubeConnectionGraph;

                checkOnce = false;
            }

            // 번호랑 라벨 찾기
            for (int i = 0; i < connectWalkable.Length; i++)
            {
                // 현재 플레이어 위치와 (부모)큐브 위치가 동일하면
                if (startCubePos == connectWalkable[i].transform.position)
                {
                    // 해당 큐브의 번호, 라벨 알기
                    curPlayerCubeNum = connectWalkable[i].GetComponent<CubeState>().cubeNum;
                    curPlayerCubeLabel = connectWalkable[i].GetComponent<CubeState>().labelNum;
                    Debug.Log("curPlayerCubeNum : " + curPlayerCubeNum);
                    connectWalkable[i].GetComponent<Renderer>().material.color
                        = new Color(255 / 255f, 0 / 255f, 255 / 255f);   //보라
				}
			}
        }
	}
}
