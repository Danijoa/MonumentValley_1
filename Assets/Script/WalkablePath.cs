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
// bfs 라벨링

/* <착시 연결> */
// 이때 착시에 필요한 큐브들은 강제로 연결시키기

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
        // [walkable]큐브(tag. ConnectWalkable)가 존재하는 큐브 받아오기
        connectWalkable = GameObject.FindGameObjectsWithTag("ConnectWalkable");
        walkableCubeNum = connectWalkable.Length;

        // 큐브 번호 연결 그래프 만들 배열 생성 : 전체 0 초기화 되어있다 (0: 비연결 / 1: 연결)
        cubeConnectionGraph = new int[walkableCubeNum, walkableCubeNum];

        // CubeState 스크립트 가져오기
        cubeState = new CubeState[walkableCubeNum];
        for (int i = 0; i < walkableCubeNum; i++)
        {
            cubeState[i] = connectWalkable[i].GetComponent<CubeState>();
        }

        // 큐 생성
        myQ = new Queue<CubeState>();

        // 시작 라벨
        label = 1;
    }

    public void MakePath()
    {
        for (int i = 0; i < walkableCubeNum; i++)
            cubeState[i].labelNum = -1;
        label = 1;

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
