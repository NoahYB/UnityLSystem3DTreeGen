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
    public static float width = 5;

    public static string[] alphabet_rules;
    public static char[] corresponding_character;

    public static string initial = "FAFFA";
    public static int generations = 3;

    public static int animation_speed = 1;

    public static Color color;
    public static bool fall = true;

    public static bool distance = true;

    public static Color[] fall_colors = { new Color(1, 0, 0, .23f), new Color(.92f, .58f, .24f, .23f), new Color(.96f, .86f, .345f, .2f) };


    public static Dictionary<Module, List<Module>> alphabet = new Dictionary<Module, List<Module>>
    {
    };
    public static Dictionary<char, string> alphabetNonParametric2 = new Dictionary<char, string>
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
    public static Dictionary<(string, bool), List<Module>> moduleAlphabet = new Dictionary<(string, bool), List<Module>>
    {
        {("A",true),  new List<Module> {new Module("A",new List<float>{0,0},(float v1, float v2) => v1 * 2, (float v1, float v2) => v1 + v2,
            (float v1, float v2) => v2 <= 3) } },
        {("A",false),  new List<Module> {new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => 0,
            (float v1, float v2) => v1 < 1),new Module("A",new List<float>{0,0},(float v1, float v2) => v1 / v2, (float v1, float v2) => 0,
            (float v1, float v2) => v2 <= 3) } },

        {("B", true), new List<Module>{C} },

        {("B", false), new List<Module> {new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => v1-1.0f, (float v1, float v2) => v1-1,
            (float v1, float v2) => v1 < 1)}}
    };


    public static List<Module> initialParametric = new List<Module> {
        new Module("B", new List<float> { 2, 4 }, (float v1, float v2) => v1, (float v1, float v2) => v2,
        (float v1, float v2) => v1 < 1),
        new Module("A", new List<float> { 1, 10 }, (float v1, float v2) => v1, (float v1, float v2) => v2,
        (float v1, float v2) => v2 <= 3)
        };
    //n = 10;
    //#define r1 0.9 /* contraction ratio for the trunk */
    //#define r2 0.6 /* contraction ratio for branches */
    //#define a0 45 /* branching angle from the trunk */
    //#define a2 45 /* branching angle for lateral axes */
    //#define d 137.5 /* divergence angle */
    //#define wr 0.707 /* width decrease rate */
    //ω : A(1,10)
    //p1: A(l, w) : *→ !(w) F(l)[&(a0) B(l* r2, w* wr)]/(d) A(l* r1, w* wr)
    //p2: B(l, w) : *→ !(w) F(l)[-(a2)$C(l* r2, w* wr)]C(l* r1, w* wr)
    //p3: C(l, w) : *→ !(w) F(l)[+(a2)$B(l* r2, w* wr)]B(l* r1, w* wr)
    public static string moduleRules = "!(w) F(l)[&(a0) B(l* r2, w* wr)]/(d) A(l* r1, w* wr)";

    public static List<Module> initialModuleForBendyTree
        = new List<Module> { new Module("A", new List<float> { 1, 10 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true) };

    public static Dictionary<(string, bool), List<Module>> moduleAlphabetForBendyTree = new Dictionary<(string, bool), List<Module>>
    {
        {("A", true), new List<Module>{
                         new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("&", new List<float> { 0, 0 }, (float v1, float v2) => 30, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => .6f * v1, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("/", new List<float> { 0, 0 }, (float v1, float v2) => 137.5f, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("A", new List<float> { 0, 0 }, (float v1, float v2) => v1*.9f, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true)
        } },
        { ("B", true), new List<Module>{
                         new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => 30, (float v1, float v2) => v2, (float v1, float v2) => true),
                         //new Module("$", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("C", new List<float> { 0, 0 }, (float v1, float v2) => .6f * v1, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("C", new List<float> { 0, 0 }, (float v1, float v2) => v1*.9f, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true)
        } },
        { ("C", true), new List<Module>{
                         new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => 30, (float v1, float v2) => v2, (float v1, float v2) => true),
                         //new Module("$", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => .6f * v1, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => v1*.9f, (float v1, float v2) => v2 * .707f, (float v1, float v2) => true)
        } }
    };

    //    n = 10
    //#define r1 0.9 /* contraction ratio 1 */
    //#define r2 0.7 /* contraction ratio 2 */
    //#define a1 10 /* branching angle 1 */
    //#define a2 60 /* branching angle 2 */
    //#define wr 0.707 /* width decrease rate */
    static float r1 = .9f;
    static float r2 = .7f;
    static float a1 = 10f;
    static float a2 = 60f;
    static float wr = .707f;
    public static List<Module> initialModuleForCircularTree
    = new List<Module> { new Module("A", new List<float> { 1, 10 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true) };
    //ω : A(1,10)
    // p1 : A(l, w) : *→ !(w) F(l)[&(a1) B(l* r1, w* wr)]
    // /(180)[&(a2) B(l* r2, w* wr)]
    //p2 : B(l, w) : *→ !(w) F(l)[+(a1)$B(l* r1, w* wr)]
    //[-(a2)$B(l* r2, w* wr)]
    public static Dictionary<(string, bool), List<Module>> moduleAlphabetForCircularTree = new Dictionary<(string, bool), List<Module>>
    {
        {("A", true), new List<Module>{
                         new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("&", new List<float> { 0, 0 }, (float v1, float v2) => a1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => r1 * v1, (float v1, float v2) => v2 * wr, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("/", new List<float> { 0, 0 }, (float v1, float v2) => 180f, (float v1, float v2) => v2, (float v1, float v2) => true),

                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("&", new List<float> { 0, 0 }, (float v1, float v2) => a2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => r2 * v1, (float v1, float v2) => v2 * wr, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true)

        } },
        {("B", true), new List<Module>{
                         new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => a1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         //new Module("$", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => r1 * v1, (float v1, float v2) => v2 * wr, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),

                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => a2, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("$", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true),
                         new Module("B", new List<float> { 0, 0 }, (float v1, float v2) => r2 * v1, (float v1, float v2) => v2 * wr, (float v1, float v2) => true),
                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true)

        } }
    };
}
