using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CubeMeshesAttatch : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        TestVert();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TestVert()
    {
        Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;
        Vector3[] cubeVertices = cubeMesh.vertices;
        Vector3[] newVerts = new Vector3[24];
        int[] cubeTriangles = cubeMesh.triangles;
        int[] newTriangles = new int[12*3];

        for(int i = 0; i < 8; i++)
        {
            newVerts[i] = cubeVertices[i];
        }
        for (int i = 0; i < 16; i++)
        {
            newVerts[i + 8] = cubeVertices[i+8];
        }

        newVerts[0].y -= .5f;

        print(cubeMesh.uv.Length);
        cubeMesh.vertices = newVerts;
        MakeTriangles(newTriangles, cubeMesh);

        cubeMesh.uv = Unwrapping.GeneratePerTriangleUV(cubeMesh);
        cubeMesh.RecalculateNormals();
        cubeMesh.RecalculateBounds();
        cubeMesh.RecalculateTangents();
        
    }
    private void MakeTriangles(int[] triangles, Mesh mesh)
    {
        int t = SetQuad(triangles, 0, 0, 1, 2, 3);
        t = SetQuad(triangles, t, 4, 5, 6, 7);
        t = SetQuad(triangles, t, 6, 7, 0, 1);
        t = SetQuad(triangles, t, 2, 3, 4, 5);
        t = SetQuad(triangles, t, 0, 2, 6, 4);
        t = SetQuad(triangles, t, 1, 7, 3, 5);
        mesh.triangles = triangles;
        
    }
    private void MakeUV(Vector3[] vertices)
    {

    }
    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }
}
