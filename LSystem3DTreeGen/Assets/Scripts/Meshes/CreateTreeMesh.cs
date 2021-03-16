using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTreeMesh : MonoBehaviour
{
    public GameObject transformPrefab;

    public GameObject point;


    private List<(Vector3, Transform)> vs;
    private List<GameObject> gs;

    List<GameObject> helpers;

    private List<Vector3> vertices;


    public struct MeshInfo
    {
        public Vector3[] vertices;
        public int[] triangles;
    }
    public MeshInfo Init(List<Transform> turtleTransforms, Vector3 initialPosition)
    {
        transformPrefab = GameObject.FindGameObjectWithTag("DummyTransform");
        List<GameObject> spheres = new List<GameObject>();


        vertices = new List<Vector3>();
        gs = new List<GameObject>();
        vs = new List<(Vector3, Transform)>();
        //print("count = " + turtleTransforms.Count);
        vs.Add((turtleTransforms[0].position, turtleTransforms[0]));
        for (int i = 0; i < turtleTransforms.Count; i++)
        {

            //print("Turtle " + i + " UP Vector = " + turtleTransforms[i].position);
            if (i == turtleTransforms.Count - 1)
            {
                vs.Add((turtleTransforms[i].position, turtleTransforms[i]));
            }

            else if (!turtleTransforms[i + 1].name.Equals("LEAF"))
            {

                Debug.DrawLine(turtleTransforms[i].position, turtleTransforms[i + 1].position, Color.cyan, 1000);
                Vector3 pos = turtleTransforms[i + 1].position - (.5f * turtleTransforms[i].up);
                Quaternion rot = turtleTransforms[i].rotation;
                Vector3 pos2 = turtleTransforms[i + 1].position + (.5f * turtleTransforms[i + 1].up);
                Quaternion rot2 = turtleTransforms[i + 1].rotation;
                Transform t = Instantiate(transformPrefab.transform, pos, rot);
                Transform t2 = Instantiate(transformPrefab.transform, pos2, rot2);
                vs.AddRange(GetBendedPoints(t, t2));
                Destroy(t.gameObject);
                Destroy(t2.gameObject);
            }
            //else
            //{
            //    Debug.DrawLine(turtleTransforms[i].position, turtleTransforms[i + 1].position, Color.cyan, 1000);
            //    Vector3 pos = turtleTransforms[i + 1].position - (.5f * Settings.line_length *turtleTransforms[i].up);
            //    Quaternion rot = turtleTransforms[i].rotation;
            //    Vector3 pos2 = turtleTransforms[i + 1].position + (turtleTransforms[i + 1].up);
            //    Quaternion rot2 = turtleTransforms[i + 1].rotation;
            //    Transform t = Instantiate(transformPrefab.transform, pos, rot);
            //    Transform t2 = Instantiate(transformPrefab.transform, pos2, rot2);
            //    vs.AddRange(GetBendedPoints(t, t2));
            //    Destroy(t.gameObject);
            //    Destroy(t2.gameObject);
            //}
            if (turtleTransforms[i].name.Equals("LEAF"))
            {
                print(turtleTransforms[i].name);
                print(turtleTransforms.Count - 1);
                print(i);
            }

        }
        int p = 0;
        foreach ((Vector3, Transform) v in vs)
        {
            GameObject ig = Instantiate(transformPrefab);
            ig.transform.position = v.Item1;
            ig.transform.rotation = v.Item2.transform.rotation;
            gs.Add(ig);
            if (p == vs.Count - 1)
            {
                //vertices.AddRange(CreateCircleAroundPoint(ig.transform, 20, 200 / Vector3.Distance(initialPosition, ig.transform.position)));
            }
            else
            {
                //vertices.AddRange(CreateCircleAroundPoint(ig.transform, 20, 300 / Vector3.Distance(initialPosition, ig.transform.position)));
            }
            vertices.AddRange(CreateCircleAroundPoint(ig.transform, 20, .02f));
            Destroy(ig);
            p++;
        }
        MeshInfo mInfo = new MeshInfo();
        mInfo.vertices = vertices.ToArray();
        mInfo.triangles = CreateTriangles(mInfo.vertices);
        for (int i = 0; i < turtleTransforms.Count; i++)
        {
            Destroy(turtleTransforms[i].gameObject);
        }
        return mInfo;
    }
    // Update is called once per frame
    void Update()
    {
    }
    int[] CreateTriangles(Vector3[] vertices)
    {
        List<int> triangleList = new List<int>();
        for (int i = 0; i < vertices.Length - 20; ++i)
        {
            //if (i >= vertices.Length - 20)
            //{
            //    triangleList.Add(i); //A
            //    triangleList.Add(i + 1);//B
            //    triangleList.Add(vertices.Length - 1);//C
            //}
            if (i % 20 == 19)
            {
                triangleList.Add(i);
                triangleList.Add(i - 19);
                triangleList.Add(i + 20);

                triangleList.Add(i - 19);
                triangleList.Add(i + 1);
                triangleList.Add(i + 20);
            }
            else
            {
                triangleList.Add(i);
                triangleList.Add(i + 1);
                triangleList.Add(i + 20);

                if (i < vertices.Length - 21)
                {
                    triangleList.Add(i + 1); //A
                    triangleList.Add(i + 21);//B
                    triangleList.Add(i + 20);//C
                }
            }

        }

        int[] triangle_array = new int[triangleList.Count];

        int j = 0;

        foreach (int triangle_point in triangleList)
        {
            triangle_array[j] = triangle_point;
            ++j;
        }
        return triangle_array;
    }
    //https://stackoverflow.com/a/25182327/13636237
    List<(Vector3, Transform tr)> GetBendedPoints(Transform tOne, Transform tTwo)
    {
        List<(Vector3, Transform)> vList = new List<(Vector3, Transform)>();
        int interpolationFactor = 10;
        float tStep = .1f;
        Vector3 N0 = tOne.transform.up;
        Vector3 N1 = tTwo.transform.up;
        Vector3 A0 = tOne.transform.position;
        Vector3 A1 = tOne.transform.up;
        Vector3 A2 = 3.0f * (tTwo.transform.position - tOne.transform.position) - N1 - 2.0f * N0;
        Vector3 A3 = N1 + N0 - 2.0f * (tTwo.transform.position - tOne.transform.position);

        for (int t = 0; t < interpolationFactor - 1; t++)
        {
            Transform trans = Instantiate(transformPrefab).transform;
            trans.rotation = Quaternion.Lerp(tOne.transform.rotation, tTwo.transform.rotation, tStep);
            Vector3 pPos = A0 + (A1 * tStep) + (A2 * tStep * tStep) + (A3 * tStep * tStep * tStep);
            vList.Add((pPos, trans));
            tStep += .1f;
            Destroy(trans.gameObject);
        }
        return vList;
    }
    List<Vector3> CreateCircleAroundPoint(Transform transform, int nbPointsInCircle, float width)
    {
        List<Vector3> vertexPositions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        Vector3 center = transform.position;
        Vector3 up = transform.up;
        Vector3 initPosition = Vector3.Cross(transform.right, up) * width;
        for (int i = 0; i < nbPointsInCircle; i++)
        {
            Quaternion q = Quaternion.AngleAxis(((float)360 / (float)nbPointsInCircle) * i, up);
            Vector3 pos = initPosition;
            pos = q * pos;
            pos += transform.position;
            vertexPositions.Add(pos);
            rotations.Add(transform.rotation);
            //GameObject p = Instantiate(transformPrefab);
            //p.transform.position = pos;
            //p.transform.rotation = transform.rotation;
            //Destroy(p);
        }
        return vertexPositions;
    }
}