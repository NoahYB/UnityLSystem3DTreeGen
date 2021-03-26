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

    private List<Vector3> verticesList;
    private List<int> trianglesList;

    private int triangleIndex;
    Transform lastTransform;
    int[] lastTransformTriangles;

    public GameObject sphere;

    private int counter = 1;
    bool connectorAdd = true;

    Dictionary<Vector3, int[]> triangleTransform;
    public struct MeshInfo
    {
        public Vector3[] vertices;
        public int[] triangles;
    }
    public void Initialize()
    {
        lastTransformTriangles = new int[0];
        triangleTransform = new Dictionary<Vector3, int[]>();
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();
        transformPrefab = GameObject.FindGameObjectWithTag("DummyTransform");
        sphere = GameObject.FindGameObjectWithTag("Sphere");
    }
    
    public void AddSegment(Transform t1, Transform t2, float w1, float w2)
    {

        //t1.transform.position +=  .1f * -t1.transform.up.normalized;
        //t2.transform.position += .1f * t1.transform.up.normalized;

        if (connectorAdd == true)
        {
            trianglesList.AddRange(AddConnector(lastTransformTriangles));
            counter += 2;
        }


        else
            connectorAdd = true;

        List<Vector3> vListOne = (CreateCircleAroundPoint(t1, 20, w1));
        List<Vector3> vListTwo = (CreateCircleAroundPoint(t2, 20, w2));
        lastTransformTriangles = new int[vListTwo.Count];
        for (int i = 0; i < vListTwo.Count; i++)
            {
                lastTransformTriangles[i] = i + 20 + verticesList.Count; // * counter;//number of sides;   
            }


        verticesList.AddRange(vListOne);
        verticesList.AddRange(vListTwo);
        lastTransform = CopyTransform(t2);
        trianglesList.AddRange(CreateTriangleList(verticesList));
        
    }
    public List<int> AddConnector(int[] lastTriangles)
    {
        List<int> triangleList = new List<int>();
        for (int i = 0; i < lastTriangles.Length; ++i)
        {
            if (true)
            {
                triangleList.Add(lastTriangles[i]);

                triangleList.Add(lastTriangles[i] + 1);

                triangleList.Add(lastTriangles[i] + 20);

                triangleList.Add(lastTriangles[i] + 21);

                triangleList.Add(lastTriangles[i] + 20);

                triangleList.Add(lastTriangles[i] + 1);

            }
        }
        return triangleList;
    }
    public MeshInfo FinishMeshCreation()
    {
        MeshInfo mInfo = new MeshInfo();
        mInfo.vertices = verticesList.ToArray();
        mInfo.triangles = trianglesList.ToArray();
        return mInfo;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void SetLastTranfsorm(Transform t)
    {
        lastTransform = t;
    }
    Transform CopyTransform(Transform t)
    {
        return Instantiate(t.transform, t.transform.position, t.transform.rotation).transform;
    }
    public void ResetTriangle(Vector3 pos)
    {
       connectorAdd = false;

    }
    List<int> CreateTriangleList(List<Vector3> vertices)
    {
        List<int> triangleList = new List<int>();
        for (int i = triangleIndex; i < vertices.Count ; ++i)
        {
            if (i < vertices.Count - 20)
            {
                triangleList.Add(i);

                triangleList.Add(i + 1);

                triangleList.Add(i + 20);
            }
            else
            {

                triangleList.Add(i - 20);
                triangleList.Add(i);
                triangleList.Add(i - 1);
            }
        }
        triangleIndex += 40;

        return triangleList;
    }
    List<(Vector3, Transform tr)> GetBendedPoints(Transform tOne, Transform tTwo)
    {
        List<(Vector3, Transform)> vList = new List<(Vector3, Transform)>();
        int interpolationFactor = 10;
        float tStep = 0;
        tOne.LookAt(tTwo.transform);
        Vector3 N0 = Vector3.up;
        Vector3 N1 = Vector3.right;
        Vector3 A0 = tOne.transform.position;
        Vector3 A1 = tOne.transform.up;
        Vector3 A2 = 3.0f * (tTwo.transform.position - tOne.transform.position) - N1 - 2.0f * N0;
        Vector3 A3 = N1 + N0 - 2.0f * (tTwo.transform.position - tOne.transform.position);

        for (int t = 0; t <= interpolationFactor; t++)
        {
            Transform trans = Instantiate(sphere).transform;
            trans.rotation = Quaternion.Lerp(tOne.transform.rotation, tTwo.transform.rotation, tStep);
            Vector3 pPos = A0 + (A1 * tStep) + (A2 * tStep * tStep) + (A3 * tStep * tStep * tStep);
            trans.position = pPos;
            vList.Add((pPos, trans));
            tStep += .1f;
            //Destroy(trans.gameObject);
        }
        return vList;
    }
    void CalculateTangents(Vector3 position1, Vector3 position2)
    {
        
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