using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    public static int cubeNumber = 0;

    public int cubeNum;     // ť�� ���� ��ȣ
    public int labelNum;    // �󺧸��Ǿ� �ִ� ��ȣ
    public bool walkable;   // �÷��̾ �̵� ������ ť���ΰ�

    Renderer colorChange;

    private void Awake()
    {
        cubeNum = cubeNumber++;
        //Debug.Log("cubeNum : " + cubeNum);
    }

    private void Start()
    {
        labelNum = -1;
        walkable = false;

        colorChange = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        // ���� ����
        if (labelNum == 1)
            colorChange.material.color = new Color(0 / 255f, 255 / 255f, 255 / 255f);   //�ϴ�
        else if (labelNum == 2)
            colorChange.material.color = new Color(255 / 255f, 0 / 255f, 255 / 255f);   //����
        else if (labelNum == 3)
            colorChange.material.color = new Color(255 / 255f, 0 / 255f, 0 / 255f);     //����
        else if (labelNum == 4)
            colorChange.material.color = new Color(0 / 255f, 0 / 255f, 255 / 255f);     //�Ķ�
        else if (labelNum == 5)
            colorChange.material.color = new Color(0 / 255f, 255 / 255f, 0 / 255f);     //�ʷ�
        else if (labelNum == 6)
            colorChange.material.color = new Color(0 / 255f, 0 / 255f, 0 / 255f);       //����
        else
            colorChange.material.color = new Color(255 / 255f, 255 / 255f, 0 / 255f);   //���
    }
}
