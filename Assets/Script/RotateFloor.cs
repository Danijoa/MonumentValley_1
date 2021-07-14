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
        // ó�� �ѹ��� ����
        if (checkOnce)
        {
            // [walkable]ť��� �޾ƿ���
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
            ConnectIllusion();
            checkOnce = false;
        }

        // ȸ�� : ���콺 �� Ŭ��
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
                // �ڵ� ��ġ�� ȭ��� ��ǥ
                floorHandlePos = Camera.main.WorldToScreenPoint(transform.position);

                // ���� ���콺 ��ġ - ���� ���콺 ��ġ
                curPos = Input.mousePosition;
                dir = curPos - prevPos;

                // �ڵ� ���� (����) ȸ�� ���� ���� -> ȸ����
                dotValue = Vector3.Dot(dir, Camera.main.transform.up);

                // x �� ȸ��
                if (curPos.x >= floorHandlePos.x) // 1,2��и�  
                {
                    // transform.Rotate(ȸ�� ���� ��, ȸ�� �ӵ�, world ��ǥ ����)
                    transform.Rotate(transform.up, -dotValue, Space.World);
                }
                else // 3,4��и� 
                {
                    transform.Rotate(transform.up, dotValue, Space.World);
                }

                prevPos = curPos;
            }
        }

        // ���� ã��
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

        // �ε巴�� ���� �̵�
        if (checkRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
            if (transform.rotation == to)
            {
                checkPlayer = false;
                walkablePath.MakePath();
            }
        }

        // �÷��̾ ���� ȸ�� ���� ����
        if (checkPlayer)
            MakePlayerMove();
    }

    private void MakePlayerMove()
    {
        // �÷��̾ ��ġ�� �ִ� walkable cube number�� ���� ť���� ��ġ���� ����
        for (int i = 0; i < walkablePath.connectWalkable.Length; i++)
        {
            if (playerMovement.playerCubeNum == walkablePath.cubeState[i].cubeNum)
            {
                playerMovement.PlayerCubePos(walkablePath.connectWalkable[i].transform.position);
            }
        }
    }
}
