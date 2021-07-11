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

			// �������� ȸ�� ����..��...
			dotValue = Vector3.Dot(dir, Camera.main.transform.up);

			// z �� ȸ��
			if (curPos.x >= handlePos.x) // 1,2��и�  
			{
				// transform.Rotate(ȸ�� ���� ��, ȸ�� �ӵ�, world ��ǥ ����)
				transform.Rotate(transform.forward, dotValue, Space.World);
			}
			else // 3,4��и� 
			{
				transform.Rotate(transform.forward, -dotValue, Space.World);
			}

			prevPos = curPos;
		}

		// ���� ã��
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

		// �ε巴�� ���� �̵�
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
	//	// ���콺 ��ġ
	//	if (Input.GetMouseButton(0))
	//	{			
	//		// 3D ����� ���콺 ��ǥ
	//		touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	//		// ȸ���� ������Ʈ ���� ��ġ
	//		standardPos = transform.position;

	//		// ������Ʈ ���� ��ǥ���� ���콺 ��ǥ���� ����
	//		dir = (touchedPos - standardPos).normalized;
	//		// ������Ʈ up ����
	//		up = new Vector3(0, standardPos.y, 0);
	//		// �� ���� ������ ���� ���ϱ� ���� ����ȭ �� ���� 
	//		rotateAngle = Vector3.Dot(dir, up.normalized);

	//		// ȸ�� ���� ����
	//		if (prevPos != touchedPos)	
	//		{
	//			// �ð� �ݴ� �������� ȸ��
	//			//if()
	//				transform.Rotate(transform.forward, rotateAngle * 4f, Space.World);
	//			// �ð� �������� ȸ��
	//			//else
	//				//transform.Rotate(transform.forward, -rotateAngle * 4f, Space.World);
	//		}

	//		// ���� Ŭ�� ��ġ
	//		prevPos = touchedPos;
	//		preAngle = rotateAngle;
	//		prevDir = dir;

	//		Debug.Log("rotateAngle : " + rotateAngle);
	//		Debug.Log("preAngle : " + preAngle);
	//		Debug.Log("dir : " + dir);
	//		Debug.Log("prevDir : " + prevDir);
	//	}
	//}
