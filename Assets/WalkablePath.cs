using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/* <길 연결> 1번 생각 */
// [walkable]큐브(tag. ConnectWalkable)가 존재하는 모든 [부모]큐브 받아오기
// [부모]큐브를 하나하나 확인하면서 아래 있는 [up down left right]큐브 파란불 들어와있는지 확인
// 파란불 있는 큐브끼리 묶기 -> 라벨링 처럼..

/* <길 연결> 2번 생각 */
// [walkable]큐브(tag. ConnectWalkable)가 존재하는 모든 [부모]큐브 받아오기
// 1번 큐브 부터 연결 그래프 확인하면서 인접 큐브 받아오기
// 라벨링

/* <착시 연결> */
// 이때 착시로 인해 나타나는 큐브는 강제로 연결시키기

/* 플레이어 이동 */
// 플레이어는 walkable 위치에서 local 좌표 기준 (y?) +0.5 만큼에 위치해 있기
// 플레이어가 위치해 있는 큐브 묶음 [walkable]큐브 색상 변경
// walkable 큐브 묶음은 가중치 없는 방향 그래프?로 존재
// 갈찾기는 a*? 프로이드와샬?

public class WalkablePath : MonoBehaviour
{
    public int[,] cubeConnectionGraph;

    private GameObject[] connectWalkable;
    private CubeState[] cubeState;
    private int walkableCubeNum;

    private CubeState curCube;
    private CubeState nextCube;
    private Queue<CubeState> myQ;
    private int label;
    private int curCubeNum;

    int illusion1, illusion2;

    //private bool[] cubeBlue = new bool[4];
    //private CubeState nextCube;

    void Start()
    {
        // [walkable]큐브(tag. ConnectWalkable)가 존재하는 큐브 받아오기
        connectWalkable = GameObject.FindGameObjectsWithTag("ConnectWalkable");
        walkableCubeNum = connectWalkable.Length;

        // 큐브 번호 연결 그래프 만들 배열 생성 : 전체 0 초기화 되어있다 (0: 비연결 / 1: 연결)
        cubeConnectionGraph = new int[walkableCubeNum, walkableCubeNum];

        // [부모]큐브의 CubeState 스크립트 가져오기
        cubeState = new CubeState[walkableCubeNum];
        for (int i = 0; i < walkableCubeNum; i++)
        {
            cubeState[i] = connectWalkable[i].transform.parent.gameObject.GetComponent<CubeState>();
        }

        // 큐 생성
        myQ = new Queue<CubeState>();

        // 시작 라벨
        label = 1;

        // 착시 연결
        ConnectIllusion();
    }

    void ConnectIllusion()
    {
        for (int i = 0; i < walkableCubeNum; i++)
        {
            if (cubeState[i].gameObject.tag == "Illusion1Up")
                illusion1 = cubeState[i].cubeNum;

            if (cubeState[i].gameObject.tag == "Illusion1Down")
                illusion2 = cubeState[i].cubeNum;
        }
        cubeConnectionGraph[illusion1, illusion2] = 1;
        cubeConnectionGraph[illusion2, illusion1] = 1;
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < walkableCubeNum; i++)
                cubeState[i].labelNum = -1;
            label = 1;
            MakePath();
        }
    }

    void MakePath()
    {
        for (int i = 0; i < walkableCubeNum; i++)
        {
            // 이미 확인된 큐브 
            if (cubeState[i].labelNum != -1)
                continue;

            // 큐에 큐브(CubeState) 넣기
            myQ.Enqueue(cubeState[i]);

            while (myQ.Count != 0)
            {
                // 현재 큐브 큐에서 큐브 빼고 반환
                curCube = myQ.Dequeue();
                // 현재 큐브 라벨 설정하기
                curCube.labelNum = label;
                // 현재 큐브 번호 받아오기
                curCubeNum = curCube.cubeNum;

                // 현재 큐브랑 연결되어 있는 큐브 번호들 그래프에서 확인하기
                // 번호에 해당하는 큐브(CubeState)가 아직 라벨링 되지 않았다면
                // 하나씩 가져오와서 라벨 설정해 주고 큐브 큐에 넣기
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

            // 다음 라벨 그륩으로 넘어가기
            label++;
        }
    }
}
