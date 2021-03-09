using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWUnity : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    Vector3 scale;
    void Start()
    {
        Mesh cube1Mesh = cube1.GetComponent<MeshFilter>().mesh;
        Mesh cube2Mesh = cube2.GetComponent<MeshFilter>().mesh;
        
        cube1.transform.position = cube2.transform.position;
        Vector3[] cube1Verts = cube1Mesh.vertices;
        Vector3[] cube2Verts = cube2Mesh.vertices;
        //cube2.transform.Rotate(new Vector3(12, 0, 60));
        cube1.transform.rotation = cube2.transform.rotation;
        scale = cube2.transform.localScale;
        UpdateVertsAllignedWithOldCube(ref cube1Verts, cube2Verts, 1, cube2.transform.up);

        cube1Mesh.vertices = cube1Verts;

        cube1Mesh.RecalculateBounds();
        cube1Mesh.RecalculateNormals();
    }
    public void UpdateVertsAllignedWithOldCube(ref Vector3[] new_verts, Vector3[] old_verts, float lineLength, Vector3 up)
    {
        // 15,7,19
        // 12,6,20
        // 14,1,16
        // 13,0,23
        new_verts[15] = old_verts[11];
        new_verts[7] = old_verts[5];
        new_verts[19] = old_verts[18];

        new_verts[12] = old_verts[10];
        new_verts[6] = old_verts[4];
        new_verts[20] = old_verts[21];

        new_verts[14] = old_verts[9];
        new_verts[1] = old_verts[3];
        new_verts[16] = old_verts[17];

        new_verts[13] = old_verts[8];
        new_verts[0] = old_verts[2];
        new_verts[23] = old_verts[22];

        new_verts[9] += lineLength * up;
        new_verts[17] += lineLength * up;
        new_verts[3] += lineLength * up;

        new_verts[8] += lineLength * up;
        new_verts[22] += lineLength * up;
        new_verts[2] += lineLength * up;

        new_verts[10] += lineLength * up;
        new_verts[21] += lineLength * up;
        new_verts[4] += lineLength * up;

        new_verts[11] += lineLength * up;
        new_verts[18] += lineLength * up;
        new_verts[5] += lineLength * up;

    }
}
