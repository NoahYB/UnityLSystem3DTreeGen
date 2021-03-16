using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LSystem
{
    string F_RULE;
    string INITIAL;
    string BUILDER;
    string SYSTEM;
    string X_RULE;
    int n;
    float delta;
    float line_length;
    Dictionary<Char, String> alphabet;

    public LSystem(string INITIAL, int n, Dictionary<Char, String> alphabet)
    {
        this.alphabet = alphabet;
        this.INITIAL = INITIAL;
        SYSTEM = INITIAL;
        this.n = n;
    }

    public String calculate_system()
    {
        //TODO: Make SYSTEM a list of string and iterate through APPEND all rules associated with given command to SYSTEM
        for(int i = 0; i < n; i++)
        {
            BUILDER = "";
            for(int j = 0; j < SYSTEM.Length; j ++)
            {
                
                if (alphabet.ContainsKey(SYSTEM[j]))
                {
                    BUILDER += (alphabet[SYSTEM[j]]);
                } 
                else
                {
                    BUILDER += (SYSTEM[j]);
                }
            }
            SYSTEM = BUILDER;
        }
        Debug.Log(SYSTEM);
        return SYSTEM;
    }

}
