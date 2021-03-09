using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerate
{
    // Start is called before the first frame update
    ArrayList points_of_contact;
    ArrayList dynamic_size_mesh_verts;

    int numberOfPointsOnRadius = 20;
    public TreeGenerate()
    {
        points_of_contact = new ArrayList();
        dynamic_size_mesh_verts = new ArrayList();
    }
    public struct meshInfoHolder
    {
        public int[] triangles;
        public Vector3[] vertices;
        public Vector2[] uv;
    }
    public List<meshInfoHolder> calculate_tree(float radius, List<StaticGenerator.mesh_info> passed_verticies)
    {
        List<meshInfoHolder> allMeshes = new List<meshInfoHolder>();
        
        int d = 0;
        List<int[]> triangleArrays = new List<int[]>();

        List<Vector3> meshVerticesList = new List<Vector3>();
        List < Mesh > meshes = new List<Mesh>();
        List<int> finalTriangleList = new List<int>();
        Vector3 lastPoint = passed_verticies[0].position - Vector3.down;
        Vector3 basisOne = Vector3.zero;
        Vector3 basisTwo = Vector3.zero;
        for (int i = 0; i<passed_verticies.Count; i++)
        {
            if (lastPoint.y <= -100000f && i != 0)
            {
                lastPoint = passed_verticies[i - 1].position;

            }
            else if (i != 0) lastPoint = passed_verticies[i].position - lastPoint;
            else lastPoint = new Vector3(passed_verticies[i].position.x,
                                         passed_verticies[i].position.y+.01f,
                                         passed_verticies[i].position.z); 
           
            Vector3 normalized = lastPoint.normalized;
            float dotProduct1 = Vector3.Dot(normalized, Vector3.left);
            float dotProduct2 = Vector3.Dot(normalized, Vector3.forward);
            float dotProduct3 = Vector3.Dot(normalized, Vector3.up);



            Vector3 dotVector = ((1.0f - Mathf.Abs(dotProduct1)) * Vector3.right) +
                                ((1.0f - Mathf.Abs(dotProduct2)) * Vector3.forward) +
                                ((1.0f - Mathf.Abs(dotProduct3)) * Vector3.up);
            Vector3 A = Vector3.Cross(normalized, dotVector.normalized);
            Vector3 B = Vector3.Cross(A, normalized);

            if (passed_verticies[i].skip)
            {
                meshInfoHolder meshInfo = new meshInfoHolder();

                int[] triangleArray = (calculate_triangle_array(dynamic_size_mesh_verts));
                Vector3[] vertexArray = meshVerticesList.ToArray();

                meshInfo.triangles = triangleArray;
                meshInfo.vertices = vertexArray;

                allMeshes.Add(meshInfo);

                dynamic_size_mesh_verts.Clear();
                meshVerticesList.Clear();

            }
            for (int j = 0; j< numberOfPointsOnRadius; ++j)
            {
                
                Vector3 currentPoint = passed_verticies[i].position;
                //x′= rcosθcosα
                //y′= rsinθ
                //z′= rcosθsinα
                float x_to_add = (passed_verticies[i].width) * Mathf.Sin(j / Mathf.PI);
                float z_to_add = (passed_verticies[i].width) * Mathf.Cos(j / Mathf.PI);
                float y_to_add = (passed_verticies[i].width) * Mathf.Cos(j / Mathf.PI);
                //Vector3 v = new Vector3(currentPoint.x + x_to_add, currentPoint.y, currentPoint.z + z_to_add);
                Vector3 v = getPointOnCircle(passed_verticies[i].width - Random.Range(0,0) + .5f, j / Mathf.PI, currentPoint,
                      B, A);
                meshVerticesList.Add(v);
                dynamic_size_mesh_verts.Add(v);
            }
            d++;

            if (!passed_verticies[i].skip) lastPoint = passed_verticies[i].position;
            else lastPoint = Vector3.negativeInfinity;
        }
        if (true){
            meshInfoHolder finalMeshInfo = new meshInfoHolder();

            int[] finalTriangleArray = (calculate_triangle_array(dynamic_size_mesh_verts));
            Vector3[] finalVertexArray = meshVerticesList.ToArray();

            finalMeshInfo.triangles = finalTriangleArray;
            finalMeshInfo.vertices = finalVertexArray;
            finalMeshInfo.uv = calculate_uv();
            allMeshes.Add(finalMeshInfo);
            meshVerticesList.Clear();
            dynamic_size_mesh_verts.Clear();
        }
        return allMeshes;
    }
    private Vector2[] calculate_uv()
    {
        Vector2[] uv = new Vector2[]{ new Vector2(0, 0) };
        return uv;
    }
    private int[] calculate_triangle_array(ArrayList dynamic_size_mesh_verts)
    {
        ArrayList new_triangles = new ArrayList();

        int p = 0;

        Vector3[] mesh_vertices = new Vector3[dynamic_size_mesh_verts.Count];

        int k = 0;

        
        foreach (Vector3 vertex in dynamic_size_mesh_verts)
        {
            mesh_vertices[k] = vertex;
            ++k;
        }
        for (int i = 0; i < mesh_vertices.Length - numberOfPointsOnRadius; ++i)
        {
            if(true)
            {
                new_triangles.Add(i);
                new_triangles.Add(i + 1);
                new_triangles.Add(i + numberOfPointsOnRadius);

                if (i < mesh_vertices.Length - 21)
                {
                    new_triangles.Add(i + 1); //A
                    new_triangles.Add(i + numberOfPointsOnRadius+1);//B
                    new_triangles.Add(i + numberOfPointsOnRadius);//C
                }
            }

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
    private Vector3 getPointOnCircle(float radius, float angle, Vector3 point, Vector3 A, Vector3 B)
    {
        return point + radius * (A * Mathf.Cos(angle) + B * Mathf.Sin(angle));
    }
}