using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingParametricLSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("START");
        ParametricLSystem pSys1 = new ParametricLSystem(Settings.initialParametric, 0, Settings.moduleAlphabet);
        print(0);
        foreach (Module s in pSys1.CalculateSystem())
        {
            print(s.CheckParametersAndName());
        }
        ParametricLSystem pSys2 = new ParametricLSystem(Settings.initialParametric, 1, Settings.moduleAlphabet);
        print(1);
        foreach (Module s in pSys2.CalculateSystem())
        {
            print(s.CheckParametersAndName());
        }
        ParametricLSystem pSys3 = new ParametricLSystem(Settings.initialParametric, 2, Settings.moduleAlphabet);
        print(2);
        foreach (Module s in pSys3.CalculateSystem())
        {
            print(s.CheckParametersAndName());
        }
        ParametricLSystem pSys4 = new ParametricLSystem(Settings.initialParametric, 3, Settings.moduleAlphabet);
        print(3);
        foreach (Module s in pSys4.CalculateSystem())
        {
            print(s.CheckParametersAndName());
        }
        ParametricLSystem pSys5 = new ParametricLSystem(Settings.initialParametric, 4, Settings.moduleAlphabet);
        print(4);
        foreach (Module s in pSys5.CalculateSystem())
        {
            print(s.CheckParametersAndName());
        }
    }

}
