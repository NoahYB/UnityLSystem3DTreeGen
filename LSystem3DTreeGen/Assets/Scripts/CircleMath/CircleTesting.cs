using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTesting : MonoBehaviour
{
    public GameObject point;
    List<Vector3> testingPoints;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            testingPoints = CreateCircleAroundPoint(this.transform, 7);
            foreach(Vector3 v in testingPoints)
            {
                GameObject p = Instantiate(point);
                p.name = i.ToString();
                p.transform.position = v;
                i++;
            }
        }

    }
    List<Vector3> CreateCircleAroundPoint(Transform transform, int nbPointsInCircle)
    {
        List<Vector3> vertexPositions = new List<Vector3>();
        Vector3 center = transform.position;
        Vector3 up = transform.up;
        Vector3 initPosition = Vector3.Cross(transform.right, up) * .2f;
        for(int i = 0; i < nbPointsInCircle; i++)
        {
            Quaternion q = Quaternion.AngleAxis(((float)360/(float)nbPointsInCircle)* i, up);
            Vector3 pos = initPosition;
            pos = q * pos;
            pos += transform.position;
            vertexPositions.Add(pos);
        }
        return vertexPositions;
    }
}
