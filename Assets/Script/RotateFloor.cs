using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFloor : MonoBehaviour
{
    private Vector3 floorHandlePos;
    private Vector3 curPos;
    private Vector3 dir;

    private Vector3 prevPos;
    private float dotValue;
    private bool clickedFirst;

    private RaycastHit rayHit = new RaycastHit();
    private Ray ray;
    private bool isFloorHandle;
    private bool checkPlayer;

    private Quaternion to;
    private bool checkRotation;

    private bool checkOnce;
    private WalkablePath walkablePath;

    private PlayerMovement playerMovement;

    private int illusion1Up, illusion1Down;

    void Start()
    {
        clickedFirst = true;
        floorHandlePos = Camera.main.WorldToScreenPoint(transform.position);
        checkRotation = false;

        isFloorHandle = false;
        checkPlayer = false;

        checkOnce = true;
    }

    void ConnectIllusion()
    {
        for (int i = 0; i < walkablePath.connectWalkable.Length; i++)
        {
            if (walkablePath.connectWalkable[i].GetComponentInParent<RoadState>().gameObject.tag == "Illusion1Up")
                illusion1Up = walkablePath.cubeState[i].cubeNum;
            if (walkablePath.connectWalkable[i].GetComponentInParent<RoadState>().gameObject.tag == "Illusion1Down")
                illusion1Down = walkablePath.cubeState[i].cubeNum;
        }
    }

    void Update()
    {
        // 처음 한번만 실행
        if (checkOnce)
        {
            // [walkable]큐브들 받아오기
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
            ConnectIllusion();
            checkOnce = false;
        }

        // 회전 : 마우스 왼 클릭
        if (Input.GetMouseButton(0))
        {
            checkRotation = false;
            if (clickedFirst)
            {
                prevPos = Input.mousePosition;
                // walkablePath.MakePath();
                clickedFirst = false;
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
            {
                if (rayHit.transform.gameObject.tag == "FloorHandle")
                {
                    isFloorHandle = true;
                    checkPlayer = true;
                }
            }

            if (isFloorHandle)
            {
                // 핸들 위치의 화면상 좌표
                floorHandlePos = Camera.main.WorldToScreenPoint(transform.position);

                // 현재 마우스 위치 - 이전 마우스 위치
                curPos = Input.mousePosition;
                dir = curPos - prevPos;

                // 핸들 벡터 (내적) 회전 방향 벡터 -> 회전량
                dotValue = Vector3.Dot(dir, Camera.main.transform.up);

                // x 축 회전
                if (curPos.x >= floorHandlePos.x) // 1,2사분면  
                {
                    // transform.Rotate(회전 기준 축, 회전 속도, world 좌표 기준)
                    transform.Rotate(transform.up, -dotValue, Space.World);
                }
                else // 3,4사분면 
                {
                    transform.Rotate(transform.up, dotValue, Space.World);
                }

                prevPos = curPos;
            }
        }

        // 각도 찾기
        if (Input.GetMouseButtonUp(0) && isFloorHandle)
        {
            isFloorHandle = false;
            clickedFirst = true;

            if (transform.eulerAngles.y >= 45 && transform.eulerAngles.y < 135)
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(90, Vector3.up);

                walkablePath.cubeConnectionGraph[illusion1Up, illusion1Down] = 1;
                walkablePath.cubeConnectionGraph[illusion1Down, illusion1Up] = 1;
            }
            else if (transform.eulerAngles.y >= 135 && transform.eulerAngles.y < 225)
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(180, Vector3.up);

                walkablePath.cubeConnectionGraph[illusion1Up, illusion1Down] = 0;
                walkablePath.cubeConnectionGraph[illusion1Down, illusion1Up] = 0;
            }
            else if (transform.eulerAngles.y >= 225 && transform.eulerAngles.y < 315)
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(270, Vector3.up);

                walkablePath.cubeConnectionGraph[illusion1Up, illusion1Down] = 0;
                walkablePath.cubeConnectionGraph[illusion1Down, illusion1Up] = 0;
            }
            else
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(0, Vector3.up);

                walkablePath.cubeConnectionGraph[illusion1Up, illusion1Down] = 0;
                walkablePath.cubeConnectionGraph[illusion1Down, illusion1Up] = 0;
            }
        }

        // 부드럽게 각도 이동
        if (checkRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
            if (transform.rotation == to)
            {
                checkPlayer = false;
                walkablePath.MakePath();
            }
        }

        // 플레이어도 같이 회전 시켜 주자
        if (checkPlayer)
            MakePlayerMove();
    }

    private void MakePlayerMove()
    {
        // 플레이어가 위치해 있는 walkable cube number와 같은 큐브의 위치벡터 전달
        for (int i = 0; i < walkablePath.connectWalkable.Length; i++)
        {
            if (playerMovement.playerCubeNum == walkablePath.cubeState[i].cubeNum)
            {
                playerMovement.PlayerCubePos(walkablePath.connectWalkable[i].transform.position);
            }
        }
    }
}
