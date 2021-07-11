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

	private Quaternion to;
	private bool check;

	private WalkablePath walkablePath;
	private bool checkOnce;

	private void Start()
	{
		clickedFirst = true;
		handlePos = Camera.main.WorldToScreenPoint(transform.position);
		check = false;

		checkOnce = true;
	}

	void Update()
	{
		// 처음 한번만 실행
		if (checkOnce)
		{
			// [walkable]큐브들 받아오기
			walkablePath = GameObject.FindObjectOfType<WalkablePath>();
			walkablePath.MakePath();
			checkOnce = false;
		}

		// 회전 : 마우스 왼 클릭
		if (Input.GetMouseButton(0))
		{
			check = false;
			if (clickedFirst)
			{
				prevPos = Input.mousePosition;
				walkablePath.MakePath();
				clickedFirst = false;
			}

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

		// 각도 찾기
		if (Input.GetMouseButtonUp(0))
		{
			clickedFirst = true;

			if (transform.eulerAngles.z >= 45 && transform.eulerAngles.z < 135)
			{
				check = true;
				to = Quaternion.AngleAxis(90, Vector3.forward);
			}
			else if (transform.eulerAngles.z >= 135 && transform.eulerAngles.z < 225)
			{
				check = true;
				to = Quaternion.AngleAxis(180, Vector3.forward);
			}
			else if (transform.eulerAngles.z >= 225 && transform.eulerAngles.z < 315)
			{
				check = true;
				to = Quaternion.AngleAxis(270, Vector3.forward);
			}
			else
			{
				check = true;
				to = Quaternion.AngleAxis(0, Vector3.forward);
			}
		}

		// 부드럽게 각도 이동
		if (check)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * 120f);
			if(transform.rotation == to)
				walkablePath.MakePath();
		}
	}
}

	//Vector3 touchedPos;
	//Vector3 prevPos = Vector3.zero;
	//Vector3 standardPos;
	//Vector3 dir;
	//Vector3 prevDir = Vector3.zero;
	//Vector3 up;
	//float rotateAngle;
	//float preAngle = 0.0f;

	// void Update()
	//{
	//	// 마우스 터치
	//	if (Input.GetMouseButton(0))
	//	{			
	//		// 3D 월드상 마우스 좌표
	//		touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	//		// 회전할 오브젝트 기준 위치
	//		standardPos = transform.position;

	//		// 오브젝트 기준 좌표에서 마우스 좌표까지 벡터
	//		dir = (touchedPos - standardPos).normalized;
	//		// 오브젝트 up 벡터
	//		up = new Vector3(0, standardPos.y, 0);
	//		// 두 벡터 사이의 각도 구하기 위해 정규화 후 내적 
	//		rotateAngle = Vector3.Dot(dir, up.normalized);

	//		// 회전 중일 때만
	//		if (prevPos != touchedPos)	
	//		{
	//			// 시계 반대 방향으로 회전
	//			//if()
	//				transform.Rotate(transform.forward, rotateAngle * 4f, Space.World);
	//			// 시계 방향으로 회전
	//			//else
	//				//transform.Rotate(transform.forward, -rotateAngle * 4f, Space.World);
	//		}

	//		// 이전 클릭 위치
	//		prevPos = touchedPos;
	//		preAngle = rotateAngle;
	//		prevDir = dir;

	//		Debug.Log("rotateAngle : " + rotateAngle);
	//		Debug.Log("preAngle : " + preAngle);
	//		Debug.Log("dir : " + dir);
	//		Debug.Log("prevDir : " + prevDir);
	//	}
	//}
