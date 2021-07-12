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

    private Quaternion to;
    private bool checkRotation;

    private WalkablePath walkablePath;
    private bool checkOnce;

    // Start is called before the first frame update
    void Start()
    {
        clickedFirst = true;
        handlePos = Camera.main.WorldToScreenPoint(transform.position);
        checkRotation = false;

        checkOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ó�� �ѹ��� ����
        if (checkOnce)
        {
            // [walkable]ť��� �޾ƿ���
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            walkablePath.MakePath();
            checkOnce = false;
        }

        // ȸ�� : ���콺 �� Ŭ��
        if (Input.GetMouseButton(0))
        {
            checkRotation = false;
            if (clickedFirst)
            {
                prevPos = Input.mousePosition;
                walkablePath.MakePath();
                clickedFirst = false;
            }

            // �ڵ� ��ġ�� ȭ��� ��ǥ
            handlePos = Camera.main.WorldToScreenPoint(transform.position);

            // ���� ���콺 ��ġ - ���� ���콺 ��ġ
            curPos = Input.mousePosition;
            dir = curPos - prevPos;

            // �ڵ� ���� (����) ȸ�� ���� ���� -> ȸ����
            dotValue = Vector3.Dot(dir, Camera.main.transform.up);

            // x �� ȸ��
            if (curPos.x >= handlePos.x) // 1,2��и�  
            {
                // transform.Rotate(ȸ�� ���� ��, ȸ�� �ӵ�, world ��ǥ ����)
                transform.Rotate(transform.right, dotValue, Space.World);
            }
            else // 3,4��и� 
            {
                transform.Rotate(transform.right, -dotValue, Space.World);
            }

            prevPos = curPos;
        }

        // ���� ã��
        if (Input.GetMouseButtonUp(0))
        {
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

        // �ε巴�� ���� �̵�
        if (checkRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
            if (transform.rotation == to)
                walkablePath.MakePath();
        }
    }
}
