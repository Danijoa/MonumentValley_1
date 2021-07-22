using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
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

	private int illusion1Up, illusion1Down;

	private void Start()
	{
		clickedFirst = true;
		handlePos = Camera.main.WorldToScreenPoint(transform.position);
		checkRotation = false;

		isHandle = false;

		checkOnce = true;
	}

	void ConnectIllusion()
	{
		for (int i = 0; i < walkableCubeNum; i++)
		{
			if (walkablePath.connectWalkable[i].GetComponentInParent<RoadState>().gameObject.tag == "Illusion1Up")
				illusion1Up = walkablePath.cubeState[i].cubeNum;
			if (walkablePath.connectWalkable[i].GetComponentInParent<RoadState>().gameObject.tag == "Illusion1Down")
				illusion1Down = walkablePath.cubeState[i].cubeNum;
		}

		walkablePath.cubeConnectionGraph[illusion1Up, illusion1Down] = 1;
		walkablePath.cubeConnectionGraph[illusion1Down, illusion1Up] = 1;
	}

	void Update()
	{
		// 처음 한번만 실행
		if (checkOnce)
		{
			// [walkable]큐브들 받아오기
			walkablePath = GameObject.FindObjectOfType<WalkablePath>();
			walkableCubeNum = walkablePath.connectWalkable.Length;

			// 착시 연결
			ConnectIllusion();

			walkablePath.MakePath();
			checkOnce = false;
		}

		// 회전 : 마우스 왼 클릭
		if (Input.GetMouseButton(0))
		{
			checkRotation = false;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
			{
				if (rayHit.transform.gameObject.tag == "Handle")
				{
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
				handlePos = Camera.main.WorldToScreenPoint(transform.position);
				curPos = Input.mousePosition;
				dir = curPos - prevPos;

				//if (-Mathf.Epsilon < dir.y && dir.y < Mathf.Epsilon)
				//	return;

				// 내적값은 회전 방향..만...
				dotValue = Vector3.Dot(dir, Camera.main.transform.up);

				// z 축 회전
				if (curPos.x >= handlePos.x) // 1,2사분면  
				{
					// transform.Rotate(회전 기준 축, 회전 속도, world 좌표 기준)
					transform.Rotate(transform.forward, dotValue, Space.World);
				}
				else // 3,4사분면 
				{
					transform.Rotate(transform.forward, -dotValue, Space.World);
				}

				prevPos = curPos;
			}
		}

		// 각도 찾기
		if (Input.GetMouseButtonUp(0) && isHandle)
		{
			isHandle = false;
			clickedFirst = true;

			if (transform.eulerAngles.z >= 45 && transform.eulerAngles.z < 135)
			{
				checkRotation = true;
				to = Quaternion.AngleAxis(90, Vector3.forward);
			}
			else if (transform.eulerAngles.z >= 135 && transform.eulerAngles.z < 225)
			{
				checkRotation = true;
				to = Quaternion.AngleAxis(180, Vector3.forward);
			}
			else if (transform.eulerAngles.z >= 225 && transform.eulerAngles.z < 315)
			{
				checkRotation = true;
				to = Quaternion.AngleAxis(270, Vector3.forward);
			}
			else
			{
				checkRotation = true;
				to = Quaternion.AngleAxis(0, Vector3.forward);
			}
		}

		// 부드럽게 각도 이동
		if (checkRotation)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
			if(transform.rotation == to)
				walkablePath.MakePath();
		}
	}
}
