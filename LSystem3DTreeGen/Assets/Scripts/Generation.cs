using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Generation : MonoBehaviour
{
    // adjustables to tree
    float width_ratio;
    float line_length;
    float delta;
    float delta_range;
    float leaf_size;
    float trunk_size;
    // builders of tree
    public GameObject turtle_transform;
    public GameObject tree;
    public GameObject leaf;
    public GameObject treeParent;
    public GameObject leafParent;

    string SYSTEM;

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
    public List<animation_info> trees_to_be_animated = new List<animation_info>();
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
    }
    public class animation_info
    {
        public string SYSTEM;
        public float delta;
        public GameObject turtle;
        public int index;
        public Stack stack;
        public float local_starting_width;
        public ArrayList vectors;
        public GameObject treeParent;
        public GameObject leafParent;
        public float delta_range;
        public float animation_speed;
        public GameObject lastPoint;
        //array_rules;
        //corresponding_character;
        public int generations;
        public float line_length;
        public float leaf_size;
        public float trunk_size;
        public float width_ratio;
        public Color color;
        public bool fall;

        public void updateVariables()
        {
            delta = Settings.delta;
            delta_range = Settings.delta_range;
            animation_speed = Settings.animation_speed;
            //array_rules = Settings.alphabet_rules;
            //corresponding_character = Settings.corresponding_character;
            generations = Settings.generations;
            line_length = Settings.line_length;
            leaf_size = Settings.leaf_size;
            local_starting_width = Settings.trunk_size;
            width_ratio = Settings.width_ratio;
            color = Settings.color;
            fall = Settings.fall;
            lastPoint = null;
        }
    }

    void Start()
    {
        delta = Settings.delta;
        delta_range = Settings.delta_range;
        animation_speed = Settings.animation_speed;
        //array_rules = Settings.alphabet_rules;
        //corresponding_character = Settings.corresponding_character;
        generations = Settings.generations;
        line_length = Settings.line_length;
        leaf_size = Settings.leaf_size;
        trunk_size = Settings.trunk_size;
        width_ratio = Settings.width_ratio;

        alphabet = new Dictionary<char, string>();
        for (int i = 0; i < array_rules.Length; ++i)
        {
            alphabet.Add(corresponding_character[i], array_rules[i]);
        }
        starting_width = turtle_transform.transform.localScale.x;
        /*Lsystem, Initializing class and getting vectors of tree
         */
        //"^*XF^*XFXF^//XFX&F+//XFX-F/X-/"
        //"^*XF^*XFX-F^//XFX&F+//XFX-F/X-/"
        //[&FFFA] //// [&FFFA] //// [&FFFA]
        //^ / X F ^ / X F X - F ^ / / X F X & F + / / X F X - F / X - /
        LSystem tree_system = new LSystem(initial, generations, alphabet);
        SYSTEM = tree_system.calculate_system();


        /*Tree Generation, Initiating Class, Getting Mesh Verts, Mesh Triangles
         */
        //TreeGenerate tree_generator = new TreeGenerate();

        //mesh_verts = tree_generator.calculate_tree_vertices(.05f,vectors);

        //mesh_triangles = tree_generator.calculate_triangle_array(mesh_verts);


        /*Mesh, Grabbing the mesh component from arbitrary GameObject, 
         * Setting Vertices and Triangles based off prev calculations 
         * Recalculating bounds of the mesh
         */

        //Mesh tree_mesh = seed.GetComponent<MeshFilter>().mesh;

        //tree_mesh.vertices = mesh_verts;

        //tree_mesh.triangles = mesh_triangles;

        //tree_mesh.RecalculateBounds();

        //tree_mesh.RecalculateNormals();

        //tree_mesh.RecalculateTangents();

        /*Debbuging statements to know length of different arrays
         */
        //print("length of vectors: " + vectors.Count.ToString());

        //print("length of mesh_verts: " + mesh_verts.Length.ToString());

        //print("Length of mesh_triangles: " + mesh_triangles.Length.ToString());

        //print("vert example: " + mesh_verts[100].ToString());
        //DrawVertices(tree_mesh);

    }
    int RandomSign()
    {
        return UnityEngine.Random.value < .5 ? 1 : -1;
    }
    public void grow_tree(GameObject turtle)
    {
        LSystem tree_system = new LSystem(Settings.initial, Settings.generations, Settings.alphabetNonParametric);
        string SYSTEM = tree_system.calculate_system();
        animation_info a_info = new animation_info();
        a_info.SYSTEM = SYSTEM;
        a_info.delta = UnityEngine.Random.Range(delta - delta_range, delta + delta_range);
        a_info.delta *= RandomSign();
        a_info.index = 0;
        a_info.turtle = turtle;
        a_info.stack = new Stack();
        a_info.local_starting_width = starting_width;
        a_info.treeParent = Instantiate(treeParent);
        a_info.leafParent = Instantiate(leafParent);

        a_info.updateVariables();

        trees_to_be_animated.Add(a_info);
        animate_system(a_info);

    }

    public void animate_system(animation_info a_info)
    {

        int index = a_info.index;
        string SYSTEM = a_info.SYSTEM;

        if (index >= SYSTEM.Length - 1)
        {
            trees_to_be_animated.Remove(a_info);
        }
        GameObject turtle = a_info.turtle;
        Stack stack = a_info.stack;
        int i = 0;

        if (a_info.index == 0)
        {
            a_info.treeParent.transform.position = Vector3.zero;
            a_info.leafParent.transform.position = Vector3.zero;
        }
        bool new_branch_hasnt_grown = true;

        while (new_branch_hasnt_grown)
        {
            index = a_info.index;
            if (index >= SYSTEM.Length - 1)
            {
                trees_to_be_animated.Remove(a_info);
                CombineMeshes(a_info.treeParent, false);
                CombineMeshes(a_info.leafParent, a_info.fall);
                new_branch_hasnt_grown = false;
            }
            if (SYSTEM[index] == 'F')
            {


                GameObject new_point = Instantiate(tree);

                turtle.transform.position += a_info.line_length * turtle.transform.up;

                new_point.transform.localScale = new Vector3(a_info.local_starting_width, a_info.line_length, a_info.local_starting_width);
                new_point.transform.rotation = turtle.transform.rotation;
                new_point.transform.up = turtle.transform.up;
                Vector3 pos = turtle.transform.position - a_info.line_length / 2 * turtle.transform.up;
                new_point.transform.position = pos;
                new_point.transform.parent = a_info.treeParent.transform;
                a_info.lastPoint = new_point;

                new_branch_hasnt_grown = false;

                //??


            }
            else if (SYSTEM[index] == 'L')
            {

                Vector3 old_pos = turtle.transform.position;

                GameObject new_point = Instantiate(leaf);

                turtle.transform.position += line_length * turtle.transform.up;
                float p = a_info.local_starting_width * a_info.leaf_size;
                new_point.transform.localScale = new Vector3(p, p, p);
                new_point.transform.rotation = turtle.transform.rotation;
                new_point.transform.up = turtle.transform.up;
                Vector3 pos = turtle.transform.position - line_length * turtle.transform.up;
                new_point.transform.position = pos;
                new_point.transform.parent = a_info.leafParent.transform;
                if (a_info.fall)
                {
                    new_point.GetComponent<Renderer>().material.color = Settings.fall_colors[UnityEngine.Random.Range(0, 3)];
                }

            }
            else if (SYSTEM[index] == 'f')
            {
                turtle.transform.position += line_length * turtle.transform.up;
            }
            else if (SYSTEM[index] == 'w')
            {
                a_info.local_starting_width *= width_ratio;
            }
            //Pitch down(&) and up(^) by delta
            else if (SYSTEM[index] == '&')
            {
                turtle.transform.Rotate(Vector3.right * UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
                //turtle.transform.rotation *= Quaternion.AngleAxis(-delta, turtle.transform.right);
            }
            else if (SYSTEM[index] == '^')
            {
                turtle.transform.Rotate(Vector3.left * UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
                //turtle.transform.rotation *= Quaternion.AngleAxis(delta, turtle.transform.right);
            }
            //roll right(/) and left(*) by delta
            else if (SYSTEM[index] == '/')
            {
                turtle.transform.Rotate(Vector3.down * UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
            }
            else if (SYSTEM[index] == '*')
            {
                turtle.transform.Rotate(Vector3.up * UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
            }

            //turn left(-) and right(+) by delta
            else if (SYSTEM[index] == '+')
            {
                turtle.transform.Rotate(Vector3.back * -UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
            }
            else if (SYSTEM[index] == '-')
            {
                turtle.transform.Rotate(Vector3.forward * -UnityEngine.Random.Range(a_info.delta - delta_range, a_info.delta + delta_range));
            }

            else if (SYSTEM[index] == '|')
            {
                Quaternion q = Quaternion.AngleAxis(180, turtle.transform.up);
                turtle.transform.rotation = q;
            }
            //Copy turtle into the stack
            else if (SYSTEM[index] == '[')
            {
                Vector3 position_to_copy;
                Quaternion rotation_to_copy;
                position_to_copy = turtle.transform.position;
                rotation_to_copy = turtle.transform.rotation;
                float width_to_copy = a_info.local_starting_width;
                turtle_info copy_info = new turtle_info();
                copy_info.position = position_to_copy;
                copy_info.rotation = rotation_to_copy;
                copy_info.width = width_to_copy;
                stack.Push(copy_info);
            }
            else if (SYSTEM[index] == ']')
            {
                i += 1;
                turtle_info new_turtle = (turtle_info)stack.Pop();
                turtle.transform.position = new_turtle.position;
                turtle.transform.rotation = new_turtle.rotation;
                a_info.local_starting_width = new_turtle.width;


            }
            a_info.index += 1;
        }

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
        parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    int j = 0;
    void Update()
    {
        if (j == Settings.animation_speed)
        {
            for (int i = 0; i < trees_to_be_animated.Count; i++)
            {
                animate_system(trees_to_be_animated[i]);
            }
            j = 0;
        }
        j++;
    }
}