using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTriangle : MonoBehaviour
{
    private bool checkRotation;
    private Quaternion to;

    private WalkablePath walkablePath;
    private bool checkOnce;

    void Start()
    {
        checkRotation = false;
        checkOnce = true;
    }

    void Update()
    {
        // ó�� �ѹ��� ����
        if (checkOnce)
        {
            // [walkable]ť��� �޾ƿ���
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            checkOnce = false;
        }

        // �ε巴�� ���� 
        if (checkRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
            if (transform.rotation == to)
            {
                checkRotation = false;
                walkablePath.MakePath();
            }
        }
    }

    public void DoRotation()
    {
        checkRotation = true;

        // z������ 90
        to = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.up);
    }
}
