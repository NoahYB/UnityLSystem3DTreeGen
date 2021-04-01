using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

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
    public GameObject leaf;
    public GameObject leafParent;
    public GameObject tree;
    public GameObject trunkGroup;
    List<Transform> pointsForLine;
    public GameObject segmentCreator;
    List<Module> SYSTEM;
    // possible mesh building
    List<mesh_info> mesh_infos = new List<mesh_info>();

    float starting_width;

    Vector3 minBounds;
    Vector3 maxBounds;

    bool finish = false;
    class turtle_info
    {
        public Vector3 position;
        public Quaternion rotation;
        public float width;
        public Transform turtleTransform;
        public float oldWidth;
        public int level;
    }
    public struct mesh_info
    {
        public Vector3 position;
        public float width;
        public bool isLeaf;
        public bool skip;
    }
    public void Init(Dictionary<(string, bool), List<Module>> alphabet, List<Module> initial, int generations)
    {
        finish = false;
        maxBounds = Vector3.zero;
        minBounds = Vector3.zero;
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in leafParent.transform)
        {
            Destroy(child.gameObject);
        }
        turtle_transform.transform.position = new Vector3(0, 0, 0);
        turtle_transform.transform.rotation = Quaternion.identity;
        pointsForLine = new List<Transform>();
        delta = Settings.delta;
        delta_range = Settings.delta_range;
        line_length = Settings.line_length;
        leaf_size = Settings.leaf_size;
        trunk_size = Settings.trunk_size;
        width_ratio = Settings.width_ratio;
        starting_width = .0009f;
        /*Lsystem, Initializing class and getting vectors of tree
         */
        ParametricLSystem parametricLSystem5 = new ParametricLSystem(initial, generations, alphabet);
        SYSTEM = parametricLSystem5.CalculateSystem();

        InterpertSystem(SYSTEM, delta, turtle_transform);


        //MODIFIED FROM THIS ANSWER https://forum.unity.com/threads/fit-object-exactly-into-perspective-cameras-field-of-view-focus-the-object.496472/
        float cameraDistance = 1.0f; // Constant factor
        
        Vector3 objectSizes = maxBounds - Vector3.zero;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        Vector3 center = new Vector3(maxBounds[0] / 2, maxBounds[1] / 2, maxBounds[2]);
        Camera.main.transform.position = center - distance * Camera.main.transform.forward;
       
    }
    void Update()
    {
        if (finish)
        {
            //transform.Rotate(Vector3.up * .02f);
            leafParent.transform.rotation = transform.rotation;//(Vector3.up * .2f);
        }
    }

    // Update is called once per frame
    int j = 0;
    public void InterpertSystem(List<Module> SYSTEM, float delta, GameObject turtle)
    {
        Vector3 heading = turtle.transform.up;
        Stack<turtle_info> stack = new Stack<turtle_info>();
        Vector3 initial_position = turtle.transform.position;
       
        int i = 0;
        int level = 0;
        mesh_info info_to_add = new mesh_info();
        info_to_add.position = turtle.transform.position;
        info_to_add.width = starting_width;
        info_to_add.isLeaf = false;
        mesh_infos.Add(info_to_add);
        //Default width if none will be provided
        float lastWidth = 20f;
        float currentWidth = 20f;
        Transform lastTurtleTransformWhenFOccured = turtle.transform;
        List<Transform> turtleTransforms = new List<Transform>();
        List<Transform> allTransformsGarbage = new List<Transform>();
        int index = 0;
        CreateTreeMesh TreeCreator = new CreateTreeMesh();

        List<float> widths = new List<float>();
        
        foreach (Module module in SYSTEM)
        {
            string name = module.GetName();
            if (name == "F")
            {

                Transform oldTurtle = CopyTransform(turtle.transform);
                allTransformsGarbage.Add(oldTurtle);
                Vector3 axis = Vector3.Cross(turtle.transform.up.normalized, tropismVector);

                float tropismAngle = Mathf.Rad2Deg * .27f * axis.magnitude;


                turtle.transform.Rotate(axis, tropismAngle);

                

                turtle.transform.position += (module.parameters[0]) * turtle.transform.up;

                Debug.DrawLine(oldTurtle.position, turtle.transform.position, UnityEngine.Random.ColorHSV(0f, .2f, .5f, 1f, 0.5f, .7f), 100f);
                if(index == 0)
                {
                    turtleTransforms.Add(oldTurtle);
                    widths.Add(currentWidth * 1.3f);
                }
                else if(level == 0)
                {
                    turtleTransforms.Add(oldTurtle);
                    widths.Add(lastWidth * 1.3f);
                }
                else
                {
                    turtleTransforms.Add(oldTurtle);
                    widths.Add(lastWidth);
                }
                UpdateBounds(turtle);
                lastTurtleTransformWhenFOccured = CopyTransform(turtle.transform);
                allTransformsGarbage.Add(lastTurtleTransformWhenFOccured);

                if(level == 0)
                {
                    turtleTransforms.Add(lastTurtleTransformWhenFOccured);
                    widths.Add(currentWidth * 1.3f);
                }
                else
                {
                    turtleTransforms.Add(lastTurtleTransformWhenFOccured);
                    widths.Add(currentWidth);
                }


                index += 1;
                lastWidth = currentWidth;
            }
            else if (name == "L")
            {
                info_to_add.position = turtle.transform.position;
                info_to_add.width = currentWidth;
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
                lastWidth = currentWidth;
                currentWidth = module.parameters[0];

            }
            //Pitch down(&) and up(^) by delta
            else if (name == "&")
            {
                turtle.transform.Rotate(Vector3.right * module.parameters[0]);
            }

            //roll right(/) and left(*) by delta
            else if (name == "/")
            {
                turtle.transform.Rotate(Vector3.up * module.parameters[0]);
            }

            //turn left(-) and right(+) by delta
            else if (name == "+")
            {
                turtle.transform.Rotate(Vector3.back * - module.parameters[0]);
            }
            else if (name == "$")
            {
                turtle.transform.Rotate(Vector3.up * Vector3.Angle(Vector3.up, turtle.transform.right) );
            }
            else if (name == "|")
            {
                turtle.transform.Rotate(Vector3.up * (180));
            }
            //Copy turtle into the stack
            else if (name == "[")
            {
                
                Vector3 position_to_copy;
                Quaternion rotation_to_copy;
                position_to_copy = turtle.transform.position;
                rotation_to_copy = turtle.transform.rotation;

                turtle_info copy_info = new turtle_info();
                copy_info.position = position_to_copy;
                copy_info.rotation = rotation_to_copy;
                copy_info.width = currentWidth;
                copy_info.oldWidth = lastWidth;
                copy_info.turtleTransform = CopyTransform(turtle.transform);
                copy_info.level = level;
                level += 1;
                allTransformsGarbage.Add(copy_info.turtleTransform);
                stack.Push(copy_info);
            }
            else if (name == "]")
            {
                i += 1;

                GameObject segment = Instantiate(tree);

                segment.transform.position = initial_position;

                TreeMesh TC = new TreeMesh();

                TreeMesh.MeshInfo sMesh = TC.Init(turtleTransforms, initial_position, widths);

                Mesh finalSegMesh = segment.GetComponent<MeshFilter>().mesh;
                finalSegMesh.vertices = sMesh.vertices;

                finalSegMesh.triangles = sMesh.triangles;

                finalSegMesh.RecalculateNormals();
                finalSegMesh.RecalculateBounds();
                segment.transform.parent = gameObject.transform;

                turtle_info turtleInfo = stack.Pop();

                turtle.transform.position = turtleInfo.position;

                turtle.transform.rotation = turtleInfo.rotation;

                level = turtleInfo.level;
                Transform t  = CopyTransform(turtle.transform);
                allTransformsGarbage.Add(t);
                turtleTransforms.Add(t);
                currentWidth = turtleInfo.width;

                lastWidth = currentWidth;

                turtleTransforms.Clear();

                widths.Clear();

            }

            i += 1;
        }


        //turtleTransforms.Add(CopyTransform(turtle.transform));
        widths.Add(currentWidth);
        turtle.transform.position = initial_position;
        GameObject finalSegment = Instantiate(tree);

        finalSegment.transform.position = initial_position;

        TreeMesh FinalTreeCreator = new TreeMesh();

        TreeMesh.MeshInfo FinalsMesh = FinalTreeCreator.Init(turtleTransforms, initial_position,widths);

        Mesh segMesh = finalSegment.GetComponent<MeshFilter>().mesh;
        segMesh.vertices = FinalsMesh.vertices;

        segMesh.triangles = FinalsMesh.triangles;

        segMesh.RecalculateNormals();
        segMesh.RecalculateBounds();
        finalSegment.transform.parent = gameObject.transform;
        this.transform.position = new Vector3(0, 0, 0);
        UpdateCamera();
        CombineMeshes(gameObject);
        CombineMeshes(leafParent);
        DestroyTurtles(allTransformsGarbage);
        finish = true;

    }
    void DestroyTurtles(List<Transform> turtles)
    {
        foreach(Transform t in turtles)
        {
            Destroy(t.gameObject);
        }
    }
    void UpdateCamera()
    {
        //MODIFIED FROM THIS ANSWER https://forum.unity.com/threads/fit-object-exactly-into-perspective-cameras-field-of-view-focus-the-object.496472/
        float cameraDistance = 1.0f; // Constant factor
        Vector3 objectSizes = maxBounds - minBounds;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        Vector3 center = new Vector3(maxBounds[0] / 2, maxBounds[1] / 2, maxBounds[2]);
        Camera.main.transform.position = center - distance * Camera.main.transform.forward;

        
    }
    void UpdateBounds(GameObject turtle)
    {
        if (turtle.transform.position.y > maxBounds.y)
        {
            maxBounds.y = turtle.transform.position.y;
        }
        if (turtle.transform.position.x > maxBounds.x)
        {
            maxBounds.x = turtle.transform.position.x;
        }
        if (turtle.transform.position.y < minBounds.y)
        {
            minBounds.y = turtle.transform.position.y;
        }
        if (turtle.transform.position.x < minBounds.x)
        {
            minBounds.x = turtle.transform.position.x;
        }
    }
    void CombineMeshes(GameObject parent)
    {
        parent.GetComponent<MeshFilter>().mesh.Clear();
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int totalVertices = 0;
        foreach(MeshFilter m in meshFilters)
        {
            totalVertices += m.mesh.vertices.Length; ///////O(1)???//. so ok
        }
        if(totalVertices > 60000)
        {
            return;
        }
        else
        {
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
            foreach (Transform child in parent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            parent.gameObject.SetActive(true);
        }

    }
    void CreateSegment(GameObject turtle, float oldWidth, float newWidth, float h)
    {
        GameObject sC = Instantiate(segmentCreator);
        sC.transform.position = turtle.transform.position;
        sC.transform.rotation = turtle.transform.rotation;
        sC.transform.parent = this.transform;
        sC.GetComponent<ProceduralCone>().DrawCone(oldWidth, newWidth, h);
    }
    Transform CopyTransform(Transform t)
    {
        return Instantiate(t.transform, t.transform.position, t.transform.rotation).transform;
    }
    public void ExportObj(string name)
    {
        string path = "/Users/nybri/Desktop";
        path = Path.Combine(path, name + ".obj");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        ObjExporter.MeshToFile(meshFilter, path);
    }
}