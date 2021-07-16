using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCube : MonoBehaviour
{
    Vector3[] myVertices = new Vector3[6];
    //Vector2[] myUV = new Vector2[6];
    int[] myTriangle;

    Mesh myMesh;
    MeshFilter myMeshFilter;
    //MeshRenderer myMeshRenderer;

    private void Awake()
    {
        myVertices[0] = new Vector3(0 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[1] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[2] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[3] = new Vector3(1 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[4] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[5] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        //myUV[0] = new Vector2(0, 0);
        //myUV[1] = new Vector2(0, 1);
        //myUV[2] = new Vector2(1, 0);
        //myUV[3] = new Vector2(1, 1);
        //myUV[4] = new Vector2(0, 0);
        //myUV[5] = new Vector2(0, 1);

        //myTriangle = new int[] { 5,1,3, 3,2,5,
        //  2,4,5, 2,0,4, 3,1,2, 1,0,2, 0,1,4, 1,5,4 };
        myTriangle = new int[] { 5,1,3, 0,4,2,
        0,2,1, 1,2,3, 2,4,3, 3,4,5, 0,1,4, 1,5,4};
    }

    private void Start() 
    {
        myMesh = new Mesh();
        myMeshFilter = gameObject.GetComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;
        //myMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        //myMeshRenderer.material = 

        myMesh.vertices = myVertices;
        //myMesh.uv = myUV;
        myMesh.triangles = myTriangle;

        myMeshFilter.mesh.RecalculateNormals();
        myMeshFilter.mesh.RecalculateTangents();
    }

}
