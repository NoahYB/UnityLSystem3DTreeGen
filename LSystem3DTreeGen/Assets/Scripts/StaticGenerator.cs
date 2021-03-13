using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class StaticGenerator : MonoBehaviour
{
    // adjustables to tree
    float width_ratio;
    float line_length;
    float delta;
    float delta_range;
    float leaf_size;
    float trunk_size;
    float l;
    public Vector3 tropismVector;
    // builders of tree
    public GameObject turtle_transform;
    public GameObject tree;
    public GameObject leaf;
    public GameObject treeParent;
    public GameObject leafParent;
    public GameObject trunkGroup;
    List<Transform> pointsForLine;
    public GameObject segmentCreator;
    string SYSTEM;

    public Transform t;

    // possible mesh building
    List<mesh_info> mesh_infos = new List<mesh_info>();

    private Vector3[] mesh_verts;
    private int[] mesh_triangles;

    float starting_width;

    //L System Rules
    public Dictionary<char, String> alphabet;
    public string[] array_rules;
    public char[] corresponding_character;
    public string initial;
    public int generations;

    //Store all trees to be animated
    //How fast to animate
    public int animation_speed;
    class turtle_info
    {
        public Vector3 position;
        public Quaternion rotation;
        public float width;
    }
    public struct mesh_info
    {
        public Vector3 position;
        public float width;
        public bool isLeaf;
        public bool skip;
        public Transform turtleTransform;
    }
    void Init()
    {
        pointsForLine = new List<Transform>();
        delta = Settings.delta;
        delta_range = Settings.delta_range;
        animation_speed = Settings.animation_speed;
        generations = Settings.generations;
        line_length = Settings.line_length;
        leaf_size = Settings.leaf_size;
        trunk_size = Settings.trunk_size;
        width_ratio = Settings.width_ratio;
        starting_width = 2f;
        /*Lsystem, Initializing class and getting vectors of tree
         */
        LSystem tree_system = new LSystem("FA", generations, Settings.alphabetNonParametric2);
        SYSTEM = tree_system.calculate_system();

        interpert_system(SYSTEM, delta, turtle_transform);
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    int j = 0;
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Init();
        }
    }
    public void interpert_system(string SYSTEM, float delta, GameObject turtle)
    {
        Vector3 heading = turtle.transform.up;
        Stack<turtle_info> stack = new Stack<turtle_info>();

        Vector3 initial_position = turtle.transform.position;

        int i = 0;

        float old_width = starting_width;

        List<Transform> turtleTransforms = new List<Transform>();
        List<Transform> newTurtleTransforms = new List<Transform>();
        int index = 0;

        Transform lastTurtleTransformWhenFOccured = turtle.transform;

        foreach (char character in SYSTEM)
        {
            if (character == 'F')
            {

                Vector3 axis = Vector3.Cross(turtle.transform.up.normalized, tropismVector);

                float tropismAngle = Mathf.Rad2Deg * .3f * axis.magnitude;

                Transform oldTurtle = CopyTransform(turtle.transform);

                turtle.transform.Rotate(axis, tropismAngle);

                turtle.transform.position += line_length * turtle.transform.up;

                turtleTransforms.Add(oldTurtle);

                Debug.DrawLine(oldTurtle.position, turtle.transform.position, Color.red, 200);

                old_width = starting_width;

                lastTurtleTransformWhenFOccured = CopyTransform(turtle.transform);
            }
            else if (character == 'L')
            {

                Vector3 old_pos = turtle.transform.position;

                GameObject new_point = Instantiate(leaf);
                Transform leafNotifier = CopyTransform(turtle.transform);
                leafNotifier.name = "LEAF";
                lastTurtleTransformWhenFOccured = leafNotifier;
                //turtleTransforms.Add(leafNotifier);

                new_point.transform.localScale = new Vector3(10f, 10f, 10f);
                new_point.transform.rotation = turtle.transform.rotation;
                new_point.transform.up = turtle.transform.up;
                Vector3 pos = turtle.transform.position;
                new_point.transform.position = pos;
                new_point.transform.parent = leafParent.transform;

            }
            else if (character == 'f')
            {
                turtle.transform.position += line_length * turtle.transform.up;

            }
            else if (character == 'w')
            {
                starting_width *= width_ratio;
            }
            else if (character == 'e')
            {
                starting_width /= width_ratio;
            }
            //Pitch down(&) and up(^) by delta
            else if (character == '&')
            {
                turtle.transform.Rotate(Vector3.right * (delta + UnityEngine.Random.Range(0, 5)));

            }
            else if (character == '^')
            {
                turtle.transform.Rotate(Vector3.left * (delta + UnityEngine.Random.Range(0, 5)));

            }
            //roll right(/) and left(*) by delta
            else if (character == '/')
            {
                turtle.transform.Rotate(Vector3.down * (delta + UnityEngine.Random.Range(0, 5)));
            }
            else if (character == '*')
            {
                turtle.transform.Rotate(Vector3.up * (delta + UnityEngine.Random.Range(0, 5)));
            }

            //turn left(-) and right(+) by delta
            else if (character == '+')
            {
                turtle.transform.Rotate(Vector3.back * -(delta + UnityEngine.Random.Range(0, 5)));
            }
            else if (character == '-')
            {
                turtle.transform.Rotate(Vector3.forward * -(delta + UnityEngine.Random.Range(0, 5)));
            }

            else if (character == '|')
            {
                turtle.transform.Rotate(Vector3.up * (180));
            }
            //Copy turtle into the stack
            else if (character == '[')
            {
                Vector3 position_to_copy;
                Quaternion rotation_to_copy;
                position_to_copy = turtle.transform.position;
                rotation_to_copy = turtle.transform.rotation;

                turtle_info copy_info = new turtle_info();
                copy_info.position = position_to_copy;
                copy_info.rotation = rotation_to_copy;
                copy_info.width = starting_width;
                stack.Push(copy_info);
            }
            else if (character == ']')
            {
                i += 1;

                turtleTransforms.Add(lastTurtleTransformWhenFOccured);

                GameObject segment = Instantiate(tree);

                segment.transform.position = initial_position;

                CreateTreeMesh TreeCreator = new CreateTreeMesh();

                CreateTreeMesh.MeshInfo sMesh = TreeCreator.Init(turtleTransforms,initial_position);

                Mesh finalSegMesh = segment.GetComponent<MeshFilter>().mesh;
                finalSegMesh.vertices = sMesh.vertices;

                finalSegMesh.triangles = sMesh.triangles;

                finalSegMesh.RecalculateNormals();
                finalSegMesh.RecalculateBounds();
                segment.transform.parent = this.transform;

                turtle_info turtleInfo = stack.Pop();

                turtle.transform.position = turtleInfo.position;

                turtle.transform.rotation = turtleInfo.rotation;

                turtleTransforms.Clear();

                //turtleTransforms.Add(turtle.transform);
                

            }

            i += 1;
            index += 1;
        }
        turtleTransforms.Add(CopyTransform(turtle.transform));
        turtle.transform.position = initial_position;
        GameObject finalSegment = Instantiate(tree);

        finalSegment.transform.position = initial_position;

        CreateTreeMesh FinalTreeCreator = new CreateTreeMesh();

        CreateTreeMesh.MeshInfo FinalsMesh = FinalTreeCreator.Init(turtleTransforms, initial_position);

        Mesh segMesh = finalSegment.GetComponent<MeshFilter>().mesh;
        segMesh.vertices = FinalsMesh.vertices;

        segMesh.triangles = FinalsMesh.triangles;

        segMesh.RecalculateNormals();
        segMesh.RecalculateBounds();



        CombineMeshes(gameObject, false);
        CombineMeshes(leafParent, false);

        ExportObj(gameObject,"tree");
        ExportObj(leafParent,"leafs");
    }
    Transform CopyTransform(Transform t)
    {
        return Instantiate(t.transform, t.transform.position, t.transform.rotation).transform;
    }
    void CombineMeshes(GameObject parent, bool fall)
    {
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        parent.GetComponent<MeshFilter>().mesh = new Mesh();
        parent.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        if (fall)
        {
            parent.GetComponent<Renderer>().material.color = Settings.fall_colors[UnityEngine.Random.Range(0, 3)];
        }
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        parent.gameObject.SetActive(true);
    }
    void CreateSegment(GameObject turtle, float oldWidth, List<Vector3> previousTop, Transform previousTurtle)
    {
        float d = 0;
        for (int j = 0; j < 10; j++)
        {
            if (j != 0) d = 22 / 10;
            else d = 0;
            GameObject c2 = Instantiate(segmentCreator);
            c2.transform.position = turtle.transform.position;

            t.transform.rotation = Quaternion.AngleAxis(d * j,turtle.transform.up);
            

            c2.transform.rotation = t.transform.rotation;

            c2.GetComponent<ProceduralCone>().DrawCone(oldWidth, starting_width, previousTop,.1f);
        }
        GameObject sC = Instantiate(segmentCreator);
        sC.transform.position = turtle.transform.position;
        sC.transform.rotation = turtle.transform.rotation;
        sC.GetComponent<ProceduralCone>().DrawCone(oldWidth, starting_width, previousTop,20);
    }
    void ExportObj(GameObject g, string name)
    {
        string path = "/Users/nybri/Desktop";
        path = Path.Combine(path, name + ".obj");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        MeshFilter meshFilter = g.GetComponent<MeshFilter>();
        ObjExporter.MeshToFile(meshFilter, path);
    }
}