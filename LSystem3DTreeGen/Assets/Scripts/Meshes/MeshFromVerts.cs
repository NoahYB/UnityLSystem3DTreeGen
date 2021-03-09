using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFromVerts : MonoBehaviour
{
    // Start is called before the first frame update
    ArrayList points_of_contact;
    public Transform dummy;
    int numberOfPointsOnRadius = 20;
    public MeshFromVerts()
    {
        points_of_contact = new ArrayList();
    }
    public struct meshInfoHolder
    {
        public int[] triangles;
        public Vector3[] vertices;
        public Vector2[] uv;
    }
    public meshInfoHolder CreateTreeMeshSegment(float radius, List<StaticGenerator.mesh_info> passedVertices)
    {

        List<Vector3> meshVerticesList = new List<Vector3>();


        for (int i = 0; i < passedVertices.Count; i++)
        {
            Transform currentTransform = passedVertices[i].turtleTransform;
            float width = passedVertices[i].width;

            for (int j = 0; j < numberOfPointsOnRadius; ++j)  
            {
                meshVerticesList.AddRange(CreateCircleAroundPoint(currentTransform,numberOfPointsOnRadius));
            }
        }
        print(meshVerticesList.Count);
        meshInfoHolder finalMeshInfo = new meshInfoHolder();

        int[] finalTriangleArray = (calculate_triangle_array(meshVerticesList));
        Vector3[] finalVertexArray = meshVerticesList.ToArray();

        finalMeshInfo.triangles = finalTriangleArray;
        finalMeshInfo.vertices = finalVertexArray;
        finalMeshInfo.uv = calculate_uv();
        meshVerticesList.Clear();

        return finalMeshInfo;
    }
    private Vector2[] calculate_uv()
    {
        Vector2[] uv = new Vector2[] { new Vector2(0, 0) };
        return uv;
    }
    private int[] calculate_triangle_array(List<Vector3> meshVertices)
    {
        ArrayList new_triangles = new ArrayList();

        int p = 0;

        Vector3[] mesh_vertices = new Vector3[meshVertices.Count];

        int k = 0;


        foreach (Vector3 vertex in meshVertices)
        {
            mesh_vertices[k] = vertex;
            ++k;
        }
        for (int i = 0; i < mesh_vertices.Length - numberOfPointsOnRadius -1 ; ++i)
        {
            new_triangles.Add(i);
            new_triangles.Add(i + 1);
            new_triangles.Add(i + numberOfPointsOnRadius);

            new_triangles.Add(i + numberOfPointsOnRadius + 1);
            new_triangles.Add(i + 1);
            new_triangles.Add(i + numberOfPointsOnRadius);

        }

        int[] triangle_array = new int[new_triangles.Count];

        int j = 0;

        foreach (int triangle_point in new_triangles)
        {
            triangle_array[j] = triangle_point;
            ++j;
        }
        return triangle_array;
    }
    List<Vector3> CreateCircleAroundPoint(Transform transform, int nbPointsInCircle)
    {
        List<Vector3> vertexPositions = new List<Vector3>();
        Vector3 center = transform.position;
        Vector3 up = transform.up;
        Vector3 initPosition = Vector3.Cross(transform.right, up);
        for (int i = 0; i < nbPointsInCircle; i++)
        {
            Quaternion q = Quaternion.AngleAxis(((float)360 / (float)nbPointsInCircle) * i, up);
            Vector3 pos = initPosition;
            pos = q * pos;
            pos += transform.position;
            vertexPositions.Add(pos);
        }
        return vertexPositions;
    }
}