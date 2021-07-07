using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [walkable]큐브가 존재하는 모든 [부모]큐브 받아오기
// [부모]큐브를 하나하나 확인하면서 아래 있는 [up down left right]큐브 파란불 들어와있는지 확인
// 파란불 있는 큐브끼리 묶기 -> 라벨링 처럼..
// 이때 착시로 인해 나타나는 큐브는 강제로 연결시키기
// 계단 부분 큐브들도 강제로 연결시키기

// 플레이어는 walkable 위치에서 local 좌표 기준 (y?) +0.5 만큼에 위치해 있기

// 플레이어가 위치해 있는 큐브 묶음 [walkable]큐브 색상 변경

// walkable 큐브 묶음은 가중치 없는 방향 그래프?로 존재
// 갈찾기는 a*? 프로이드와샬?

public class WalkablePath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
