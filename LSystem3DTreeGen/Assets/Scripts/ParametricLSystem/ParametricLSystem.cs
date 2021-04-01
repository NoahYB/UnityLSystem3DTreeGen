using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParametricLSystem : MonoBehaviour
{
    List<Module> INITIAL;
    List<Module> BUILDER;
    List<Module> SYSTEM;
    int n;
    Dictionary<(string,bool), List<Module>> alphabet;

    public ParametricLSystem(List<Module> INITIAL, int n, Dictionary<(string,bool), List<Module>> alphabet)
    {
        this.alphabet = alphabet;
        this.INITIAL = INITIAL;
        SYSTEM = INITIAL;
        this.n = n;
    }

    public List<Module> CalculateSystem()
    {
        //TODO: Make SYSTEM a list of string and iterate through APPEND all rules associated with given command to SYSTEM
        for (int i = 0; i < n; i++)
        {
            BUILDER = new List<Module> { };
            for (int j = 0; j < SYSTEM.Count; j++)
            {
                foreach(Module Succesor in SYSTEM[j].ReturnSuccecors(alphabet))
                {
                    BUILDER.Add(Succesor);
                    if(Succesor.GetName() == "F" && Succesor.parameters[1] <= 6)
                    {
                        BUILDER.Add(new Module("L", new List<float> { 0, 0 }, (float v1, float v2) => v1 / 1.456f, (float v1, float v2) => v2, (float v1, float v2) => true));  
                    }
                }
            }
            SYSTEM = BUILDER;
        }
        return SYSTEM;
    }
}
