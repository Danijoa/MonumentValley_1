using UnityEngine;

public class SlopeCube : MonoBehaviour
{
    Vector3[] myVertices = new Vector3[24];
    int[] myTriangle;

    Mesh myMesh;
    MeshFilter myMeshFilter;

    private void Awake()
    {
        myVertices[0] = new Vector3(0 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[1] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[2] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);

        myVertices[3] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[4] = new Vector3(1 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[5] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);

        myVertices[6] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[7] = new Vector3(1 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[8] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        myVertices[9] = new Vector3(0 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[10] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[11] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        myVertices[12] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[13] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[14] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        myVertices[15] = new Vector3(0 - 0.5f, 1 - 0.5f, 0 - 0.5f);
        myVertices[16] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[17] = new Vector3(1 - 0.5f, 1 - 0.5f, 0 - 0.5f);

        myVertices[18] = new Vector3(0 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[19] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[20] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);

        myVertices[21] = new Vector3(1 - 0.5f, 0 - 0.5f, 0 - 0.5f);
        myVertices[22] = new Vector3(1 - 0.5f, 0 - 0.5f, 1 - 0.5f);
        myVertices[23] = new Vector3(0 - 0.5f, 0 - 0.5f, 1 - 0.5f);

        myTriangle = new int[] { 0,2,1, 5,3,4, 6,7,8, 9,11,10, 
            13,14,12, 16,17,15, 18,20,19, 21,22,23};
    }

    private void Start() 
    {
        myMesh = new Mesh();
        myMeshFilter = gameObject.GetComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;

        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangle;

        myMeshFilter.mesh.RecalculateNormals();
        myMeshFilter.mesh.RecalculateTangents();
    }

}
