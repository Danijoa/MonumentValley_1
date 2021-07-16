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
        // 처음 한번만 실행
        if (checkOnce)
        {
            // [walkable]큐브들 받아오기
            walkablePath = GameObject.FindObjectOfType<WalkablePath>();
            checkOnce = false;
        }

        // 부드럽게 각도 
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

        // z축으로 90
        to = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.up);
    }
}
