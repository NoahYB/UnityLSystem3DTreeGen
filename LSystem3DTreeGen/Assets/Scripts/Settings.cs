using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float width_ratio = .7f;
    public static float line_length = 40;
    public static float delta = 45f;
    public static float delta_range = 30f;
    public static float leaf_size = 4;
    public static float trunk_size = .005f; 

    public static string[] alphabet_rules;
    public static char[] corresponding_character;

    public static string initial = "FAFFA";
    public static int generations = 4;

    public static int animation_speed = 1;

    public static Color color;
    public static bool fall = true;

    public static Color[] fall_colors = {new Color(1,0,0,.23f), new Color(.92f, .58f, .24f, .23f), new Color(.96f, .86f, .345f,.2f)};
    
    
    public static Dictionary<Module, List<Module>> alphabet = new Dictionary<Module, List<Module>>
    {
    };
    public static Dictionary<char, string> alphabetNonParametric2 = new Dictionary< char, string>
    {
        {'A',  "[&FL!A]/////’[&FL!A]///////’[&FL!A]//////’[&FL!A]" },
        { 'F', "S ///// F" },
        { 'S', "F" },
        { 'L', "[’’’^^-f+f+f-|-f+f+f]" }
    };
    public static Dictionary<char, string> alphabetNonParametric = new Dictionary<char, string>
    {
        {'A',  "[&FL!A]/+////’[&FL!A]/////--//’[&FL!A]///+///’[&FL!A]" },
        { 'F', "S ///// F" },
        { 'S', "F" },
        { 'L', "[L’’’^^-f+f+f-|-f+f+f]" }
    };
    //w. ! : B(2)A(4; 4)
    //p1: A(x; y) : y <= 3! A(x * 2; x + y)
    //p2: A(x; y) : y > 3! B(x)A(x = y; 0)
    //p3: B(x) : x < 1! C
    //p4 : B(x) : x >= 1! B(x-1)
    static Module B = new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => 0,
            (float v1, float v2) => v1 < 1);
    static Module C = new Module("C", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => 0,
            (float v1, float v2) => false);
    public static Dictionary<(string, bool), List<Module>> moduleAlphabet = new Dictionary<(string,bool), List<Module>>
    {
        {("A",true),  new List<Module> {new Module("A",new List<float>{0,0},(float v1, float v2) => v1 * 2, (float v1, float v2) => v1 + v2,
            (float v1, float v2) => v2 <= 3) } },
        {("A",false),  new List<Module> {B,new Module("A",new List<float>{0,0},(float v1, float v2) => v1 / v2, (float v1, float v2) => 0,
            (float v1, float v2) => v2 <= 3) } },

        {("B", true), new List<Module>{C} },

        {("B", false), new List<Module> {new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => v1-1, (float v1, float v2) => v2,
            (float v1, float v2) => v1 < 1)}}
    };
    public static List<Module> initialParametric = new List<Module> { 
        new Module("B", new List<float> { 2, 4 }, (float v1, float v2) => v1, (float v1, float v2) => v2,
        (float v1, float v2) => v1 < 1),
        new Module("A", new List<float> { 4, 4 }, (float v1, float v2) => v1, (float v1, float v2) => v2,
        (float v1, float v2) => v2 <= 3)
        };

}
