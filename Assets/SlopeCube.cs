using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCube : MonoBehaviour
{
    Vector3[] myVertices = new Vector3[6];
    int[] myTriangle;

    Mesh myMesh;
    MeshFilter myMeshFilter;

    private void Awake()
    {
        myVertices[0] = new Vector3(0 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[1] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[2] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[3] = new Vector3(1 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[4] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[5] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        myTriangle = new int[] { 5,1,3, 3,2,5,
        2,4,5, 2,0,4, 3,1,2, 1,0,2, 0,1,4, 1,5,4 };
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
