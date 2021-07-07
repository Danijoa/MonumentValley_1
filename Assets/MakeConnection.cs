using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// trriger enter 된 순간 (1) 색상 바꿔주기 (2) 연결해주기
// trriger exit 된 순간 (1) 원래 색상으로 돌려주기 (2) 연결 해제

public class MakeConnection : MonoBehaviour
{
    Renderer connectionCube;
    Color startColor;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ConnectionIndicator")
        {
            Debug.Log(" enter " + Time.time);
            connectionCube.material.color = new Color(28 / 255f, 100 / 255f, 255 / 255f); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        connectionCube.material.color = startColor;
        Debug.Log(" exit " + Time.time);
    }

    void Start()
    {
        connectionCube = gameObject.GetComponent<Renderer>();
        startColor = connectionCube.material.color;
    }

    void Update()
    {
    }
}