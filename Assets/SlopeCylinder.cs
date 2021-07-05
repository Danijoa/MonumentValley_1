using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCylinder : MonoBehaviour
{
    Vector3[] myVertices = new Vector3[40];
    int[] myTriangle;

    Mesh myMesh;
    MeshFilter myMeshFilter;

    private void Awake()
    {
        float x, y, z;

        // 윗면 vertex
        for (int i = 0; i < 20; i++)
        {
            // 18도 씩 증가
            x = Mathf.Cos(18f * i * Mathf.PI / 180f);
            z = Mathf.Sin(18f * i * Mathf.PI / 180f);

            // 내려가기
            if (i < 10)
                y = 1 - i * 0.2f;
            // 올라가기
            else
                y = -1 + (i - 10) * 0.2f;

            myVertices[i] = new Vector3(x, y, z); 
        }

        // 밑면 vertex
        for (int i = 20; i < 40; i++)
        {
            x = Mathf.Cos(18f * (i- 20) * Mathf.PI / 180f);
            z = Mathf.Sin(18f * (i - 20) * Mathf.PI / 180f);
            y = -1;

            myVertices[i] = new Vector3(x, y, z);
        }

        // 삼각형
        myTriangle = new int[] { 
            //0,21,20, 0,1,21,
            //1,22,21, 1,2,22, 
            //2,23,22, 2,3,23,
            //3,24,23, 3,4,24,
            //4,25,24, 4,5,25,
            //5,26,25, 5,6,26,
            //6,27,26, 6,7,27,
            //7,28,27, 7,8,28,
            //8,29,28, 8,9,29,
            //9,23,29, 9,10,30,
            10,31,30, 10,31,11,
            11,32,31, 11,32,12,
            12,33,32, 12,33,13,
            13,34,33, 13,34,14,
            14,35,34, 14,35,15,
            15,36,35, 15,36,16,
            16,37,36, 16,37,17,
            17,38,37, 17,38,18,
            18,39,38, 18,39,19,
            19,20,39, 19,20,0
            };
    }

    void Start()
    {
        myMesh = new Mesh();
        myMeshFilter = gameObject.GetComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;

        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangle;
    }

}
