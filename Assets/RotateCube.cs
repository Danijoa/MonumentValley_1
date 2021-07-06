using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
	Vector3 handlePos;
	Vector3 curPos;
	Vector3 dir;

	Vector3 prevPos;
	float dotValue;
	bool clickedFirst;

	Vector3 temp;
	bool check;

	private void Start()
	{
		clickedFirst = true;
		handlePos = Camera.main.WorldToScreenPoint(transform.position);
		check = false;
	}

	void Update()
	{
		// ���콺 stay
		if (Input.GetMouseButton(0))
		{
			check = false;
			if (clickedFirst)
			{
				prevPos = Input.mousePosition;
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

		// ���콺 up
		if (Input.GetMouseButtonUp(0))
		{
			clickedFirst = true;

			if (transform.eulerAngles.z >= 45 && transform.eulerAngles.z < 135)
			{
				check = true;

				temp = new Vector3(7, 4, -1);

				//temp = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90);

				//transform.rotation = Quaternion.Lerp(transform.rotation, a, Time.time * 2f);
				//transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
				//transform.eulerAngles.y, 90);
			}
			else if (transform.eulerAngles.z >= 135 && transform.eulerAngles.z < 225)
			{
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
					transform.eulerAngles.y, 180);
			}
			else if (transform.eulerAngles.z >= 225 && transform.eulerAngles.z < 315)
			{
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
					transform.eulerAngles.y, 270);
			}
			else
			{
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
					transform.eulerAngles.y, 0);
			}
		}


		if (check)
		{

			Vector3 dir = temp - new Vector3(0, 4, -1);

			Quaternion q = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * 120f);
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
