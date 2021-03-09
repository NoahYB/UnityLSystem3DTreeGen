using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ConeTesting : MonoBehaviour
{
    public GameObject cone;
    public Transform t; 
    public Vector3[] v;
    // Start is called before the first frame update
    void Start()
    {
        v = new Vector3[0];
        for (int i = 0; i < 2; i++)
        {

            GameObject c = Instantiate(cone);
            t.transform.position += 10 * t.transform.up * i;

            float d = 0;
            for(int j = 0; j < 10; j++)
            {
                if (j != 0) d = 22 / 10;
                else d = 0;
                GameObject c2 = Instantiate(cone);
                c2.transform.position = t.transform.position;

                t.transform.Rotate(Vector3.right * d);

                c2.transform.rotation = t.transform.rotation;

                v = c2.GetComponent<ConeTestingScript>().DrawCone(1, 1, v, c2.transform.position, .1f);
            }


            c.transform.position = t.transform.position;
            c.transform.rotation = t.transform.rotation;

            v = c.GetComponent<ConeTestingScript>().DrawCone(1, 1, v, t.transform.position,10);

            Mesh mesh = c.GetComponent<MeshFilter>().mesh;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
