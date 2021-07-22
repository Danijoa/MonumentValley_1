using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStair : MonoBehaviour
{
    private Vector3 handlePos;
    private Vector3 curPos;
    private Vector3 dir;

    private Vector3 prevPos;
    private float dotValue;
    private bool clickedFirst;

    private RaycastHit rayHit = new RaycastHit();
    private Ray ray;
    private bool isHandle;

    private Quaternion to;
    private bool checkRotation;

    private bool checkOnce;
    private WalkablePath walkablePath;
    private int walkableCubeNum;

    void Start()
    {
        clickedFirst = true;
        handlePos = Camera.main.WorldToScreenPoint(transform.position);
        checkRotation = false;

        isHandle = false;

        checkOnce = true;
    }

    void Update()
    {
        // 처음 한번만 실행
        if (checkOnce)
        {
            // [walkable]큐브들 받아오기
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            walkableCubeNum = walkablePath.connectWalkable.Length;
            walkablePath.MakePath();
            checkOnce = false;
        }

        // 회전 : 마우스 왼 클릭
        if (Input.GetMouseButton(0) && GameManagerStage2.instance.isFloorRotating == false)
        {
            checkRotation = false;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
            {
                if (rayHit.transform.gameObject.tag == "Handle")
                {
                    GameManagerStage2.instance.isStairRotating = true;

                    if (clickedFirst)
                    {
                        prevPos = Input.mousePosition;
                        clickedFirst = false;
                    }

                    isHandle = true;
                }
            }

            if (isHandle)
            {
                // 핸들 위치의 화면상 좌표
                handlePos = Camera.main.WorldToScreenPoint(transform.position);

                // 현재 마우스 위치 - 이전 마우스 위치
                curPos = Input.mousePosition;
                dir = curPos - prevPos;

                // 핸들 벡터 (내적) 회전 방향 벡터 -> 회전량
                dotValue = Vector3.Dot(dir, Camera.main.transform.up);

                // x 축 회전
                if (curPos.x >= handlePos.x) // 1,2사분면  
                {
                    // transform.Rotate(회전 기준 축, 회전 속도, world 좌표 기준)
                    transform.Rotate(transform.right, dotValue, Space.World);
                }
                else // 3,4사분면 
                {
                    transform.Rotate(transform.right, -dotValue, Space.World);
                }

                prevPos = curPos;
            }
        }

        // 각도 찾기
        if (Input.GetMouseButtonUp(0) && isHandle)
        {
            GameManagerStage2.instance.isStairRotating = false;
            isHandle = false;
            clickedFirst = true;

            if (transform.eulerAngles.x >= 0 && transform.eulerAngles.x < 180)
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(90, Vector3.right);
            }
            else
            {
                checkRotation = true;
                to = Quaternion.AngleAxis(270, Vector3.right);
            }
        }

        // 부드럽게 각도 이동
        if (checkRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
            if (transform.rotation == to)
                walkablePath.MakePath();
        }
    }
}
