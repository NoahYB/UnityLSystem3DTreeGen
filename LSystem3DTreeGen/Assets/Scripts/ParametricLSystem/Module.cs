using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//w. ! : B(2)A(4; 4)
//p1: A(x; y) : y <= 3! A(x * 2; x + y)
//p2: A(x; y) : y > 3! B(x)A(x = y; 0)
//p3: B(x) : x < 1! C
//p4 : B(x) : x >= 1! B(x-1)


//List<Module> A = new Module(new List<float> {4,4},
public class Module
{


    public List<float> parameters;
    string name;
    Func<float, float, float> paramOneOperator;
    Func<float, float, float> paramTwoOperator;
    Func<float, float, bool> conditional;
    public Module(string name, List<float> parameters, Func<float, float, float> paramOneOperator,
                                                       Func<float, float, float> paramTwoOperator,
                                                       Func<float, float, bool> conditional)
    {
        this.parameters = parameters;
        this.name = name;
        this.paramOneOperator = paramOneOperator;
        this.paramTwoOperator = paramTwoOperator;
        this.conditional = conditional;
    }
    public string GetName()
    {
        return name;
    }
    public string CheckParametersAndName()
    {
        return "Module Name is: " + this.name + " Parameters are " + this.parameters[0].ToString() + ", " + this.parameters[1].ToString();
    }
    public bool CheckConditional(float v1, float v2)
    {
        return conditional(v1, v2);
    }
    List<float> UpdateSuccesorParams(float v1, float v2)
    {
        return new List<float> { paramOneOperator(v1, v2), paramTwoOperator(v1, v2) };
    }
    public List<Module> ReturnSuccecors(Dictionary<(string, bool), List<Module>> alphabet)
    {
        List<Module> successors = new List<Module>();
        
        (string, bool) key = (name, CheckConditional(parameters[0], parameters[1]));
        
        if (alphabet.ContainsKey(key))
        {
            foreach (Module succesor in alphabet[key])
            {
                Module s = CopyModule(succesor);
                s.parameters = s.UpdateSuccesorParams(parameters[0], parameters[1]);
                successors.Add(s);
            }
        }
        else
        {
            successors.Add(CopyModule(this));
        }
            

        return successors;
    }
    private Module CopyModule(Module m)
    {
        Module nm = new Module(m.name, m.parameters, m.paramOneOperator, m.paramTwoOperator, m.conditional);
        return nm;
    }
}





//IGNORE
    //public class Operation
    //{
    //    string firstParameter; string firstOperation; string firstParameterModifier;
    //    string secondParameter; string secondOperation; string secondParameterModifier;
    //    public Operation(string firstParameter, string firstOperation, string firstParameterModifier,
    //                     string secondParameter, string secondOperation, string secondParameterModifier)
    //    {
    //        this.firstParameter = firstParameter;
    //        this.firstOperation = firstOperation;
    //        this.firstParameterModifier = firstParameterModifier;
    //        this.secondParameter = secondParameter;
    //        this.secondOperation = secondOperation;
    //        this.secondParameterModifier = secondParameterModifier;
    //    }
    //    public List<float> ExecuteOperation()
    //    {
    //        float x = float.Parse(firstParameter, System.Globalization.CultureInfo.InvariantCulture);
    //        float xMod = float.Parse(firstParameterModifier, System.Globalization.CultureInfo.InvariantCulture);

    //        float y = float.Parse(secondParameter, System.Globalization.CultureInfo.InvariantCulture);
    //        float yMod = float.Parse(secondParameterModifier, System.Globalization.CultureInfo.InvariantCulture);

    //        return new List<float> { FindOperand(firstParameterModifier,x,xMod), FindOperand(secondParameterModifier, y, yMod) };
    //    }
    //    public float FindOperand(string op, float v1, float v2)
    //    {
    //        switch (op)
    //        {
    //            case "+":
    //                return v1 + v2;
    //            case "-":
    //                return v1 - v2;
    //            case "/":
    //                return v1 / v2;
    //            case "*":
    //                return v1 * v2;
    //        }
    //        return v1 + v2;
    //    }
    //}
    //// "[0]/2" or "[1]/[0]" examples of operation on parameters
