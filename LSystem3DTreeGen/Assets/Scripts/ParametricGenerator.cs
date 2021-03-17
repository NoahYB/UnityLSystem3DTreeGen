using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ParametricGenerator : MonoBehaviour
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
    List<Module> SYSTEM;

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
        public Transform turtleTransform;
    }
    public struct mesh_info
    {
        public Vector3 position;
        public float width;
        public bool isLeaf;
        public bool skip;
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
        starting_width = .082f;
        /*Lsystem, Initializing class and getting vectors of tree
         */
        ParametricLSystem parametricLSystem = new ParametricLSystem(Settings.initialModuleForCircularTree, 10, Settings.moduleAlphabetForCircularTree);
        SYSTEM = parametricLSystem.CalculateSystem();

        InterpertSystem(SYSTEM, delta, turtle_transform);
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
    public void InterpertSystem(List<Module> SYSTEM, float delta, GameObject turtle)
    {
        Vector3 heading = turtle.transform.up;
        Stack<turtle_info> stack = new Stack<turtle_info>();
        Vector3 initial_position = turtle.transform.position;

        int i = 0;
        mesh_info info_to_add = new mesh_info();
        info_to_add.position = turtle.transform.position;
        info_to_add.width = starting_width;
        info_to_add.isLeaf = false;
        mesh_infos.Add(info_to_add);
        float old_width = starting_width;

        Transform lastTurtleTransformWhenFOccured = turtle.transform;
        List<Transform> turtleTransforms = new List<Transform>();
        int index = 0;
        foreach (Module module in SYSTEM)
        {
            string name = module.GetName();
            print("NAME OF ACTION = " + name);
            if (name == "F")
            {

                Transform oldTurtle = CopyTransform(turtle.transform);
                Vector3 axis = Vector3.Cross(turtle.transform.up.normalized, tropismVector);

                float tropismAngle = Mathf.Rad2Deg * .27f * axis.magnitude;

                print(tropismAngle);

                turtle.transform.Rotate(axis, tropismAngle);

                CreateSegment(turtle, old_width, module.parameters[0] * 3);

                turtle.transform.position += (module.parameters[0] * 3) * turtle.transform.up;

                Debug.DrawLine(oldTurtle.position, turtle.transform.position, Color.red, 100f);

                old_width = starting_width;

                turtleTransforms.Add(oldTurtle);

                lastTurtleTransformWhenFOccured = CopyTransform(turtle.transform);
            }
            else if (name == "L")
            {
                info_to_add.position = turtle.transform.position;
                info_to_add.width = starting_width;
                info_to_add.isLeaf = true;
                mesh_infos.Add(info_to_add);
                Vector3 old_pos = turtle.transform.position;

                GameObject new_point = Instantiate(leaf);

                new_point.transform.localScale = new Vector3(.1f, .1f, .1f);
                new_point.transform.rotation = turtle.transform.rotation;
                new_point.transform.up = turtle.transform.up;
                new_point.transform.position = turtle.transform.position;
                new_point.transform.parent = leafParent.transform;

            }
            else if (name == "f")
            {
                turtle.transform.position += line_length * turtle.transform.up;
            }
            else if (name == "!")
            {
                starting_width *= width_ratio;
            }
            //Pitch down(&) and up(^) by delta
            else if (name == "&")
            {
                turtle.transform.Rotate(Vector3.right * (module.parameters[0] + UnityEngine.Random.Range(0, 5)));
            }

            //roll right(/) and left(*) by delta
            else if (name == "/")
            {
                turtle.transform.Rotate(Vector3.down * (module.parameters[0] + UnityEngine.Random.Range(0, 5)));
            }

            //turn left(-) and right(+) by delta
            else if (name == "+")
            {
                turtle.transform.Rotate(Vector3.back * -(module.parameters[0] + UnityEngine.Random.Range(0, 5)));
            }
            else if (name == "$")
            {
                //turtle.transform.right = Vector3.Cross(turtle.transform.right, Vector3.up) / Vector3.Cross(turtle.transform.right, Vector3.up).magnitude;
                //turtle.transform.up = Vector3.Cross(turtle.transform.right, turtle.transform.up);
                //turtle.transform.right = Vector3.RotateTowards(turtle.transform.right, Vector3.right,7f,2f);
            }
            else if (name == "|")
            {
                turtle.transform.Rotate(Vector3.up * (180));
            }
            //Copy turtle into the stack
            else if (name == "[")
            {
                turtleTransforms.Add(lastTurtleTransformWhenFOccured);
                Vector3 position_to_copy;
                Quaternion rotation_to_copy;
                position_to_copy = turtle.transform.position;
                rotation_to_copy = turtle.transform.rotation;

                turtle_info copy_info = new turtle_info();
                copy_info.position = position_to_copy;
                copy_info.rotation = rotation_to_copy;
                copy_info.width = starting_width;
                copy_info.turtleTransform = CopyTransform(turtle.transform);
                stack.Push(copy_info);
            }
            else if (name == "]")
            {
                i += 1;

                turtleTransforms.Add(lastTurtleTransformWhenFOccured);

                //GameObject segment = Instantiate(tree);

                //segment.transform.position = initial_position;

                //CreateTreeMesh TreeCreator = new CreateTreeMesh();

                //CreateTreeMesh.MeshInfo sMesh = TreeCreator.Init(turtleTransforms, initial_position);

                //Mesh finalSegMesh = segment.GetComponent<MeshFilter>().mesh;
                //finalSegMesh.vertices = sMesh.vertices;

                //finalSegMesh.triangles = sMesh.triangles;

                //finalSegMesh.RecalculateNormals();
                //finalSegMesh.RecalculateBounds();
                //segment.transform.parent = this.transform;
                turtle_info turtleInfo = stack.Pop();

                turtle.transform.position = turtleInfo.position;

                turtle.transform.rotation = turtleInfo.rotation;

                turtle.transform.up = turtleInfo.turtleTransform.up;

                starting_width = turtleInfo.width;

                turtleTransforms.Clear();
            }

            i += 1;
            index += 1;
        }
        CombineMeshes(leafParent, false);
        //turtleTransforms.Add(CopyTransform(turtle.transform));
        //turtle.transform.position = initial_position;
        //GameObject finalSegment = Instantiate(tree);

        //finalSegment.transform.position = initial_position;

        //CreateTreeMesh FinalTreeCreator = new CreateTreeMesh();

        //CreateTreeMesh.MeshInfo FinalsMesh = FinalTreeCreator.Init(turtleTransforms, initial_position);

        //Mesh segMesh = finalSegment.GetComponent<MeshFilter>().mesh;
        //segMesh.vertices = FinalsMesh.vertices;

        //segMesh.triangles = FinalsMesh.triangles;

        //segMesh.RecalculateNormals();
        //segMesh.RecalculateBounds();
        //finalSegment.transform.parent = this.transform;
        //this.transform.position = new Vector3(0, 0, 0);

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
    void CreateSegment(GameObject turtle, float oldWidth, float h)
    {
        GameObject sC = Instantiate(segmentCreator);
        sC.transform.position = turtle.transform.position;
        sC.transform.rotation = turtle.transform.rotation;
        sC.GetComponent<ProceduralCone>().DrawCone(starting_width, starting_width, h);
    }
    Transform CopyTransform(Transform t)
    {
        return Instantiate(t.transform, t.transform.position, t.transform.rotation).transform;
    }
}