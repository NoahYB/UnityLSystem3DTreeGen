using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuForStaticGen : MonoBehaviour
{
    Dictionary<(string, bool), List<Module>> alphabet;
    List<Module> initial;
    int generations;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetGenerations(string gens)
    {
        generations = int.Parse(gens);
        
       
    }
    public void ExportObj()
    {
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricGenerator>().ExportObj("ExportedObj");
    }
    public void SetAlphabet(int option)
    {
        if (option == 0)
        {
            alphabet = Settings.moduleAlphabetForBendyTree;
            initial = Settings.initialModuleForBendyTree;
            
        }
        if (option == 1)
        {
            alphabet = Settings.moduleAlphabetForSpaceFillingLine;
            initial = Settings.initialModuleForSpaceFillingLine;
        }
        if (option == 2)
        {
            alphabet = Settings.moduleAlphabetForWeepingTree;
            initial = Settings.initialModuleForWeepingTree;
        }
        if (option == 3)
        {
            alphabet = Settings.moduleAlphabetForCircularTree;
            initial = Settings.initialModuleForCircularTree;
        }
    }
    public void Generate()
    {
        print(generations);
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricAnimated>().Init(alphabet, initial, generations);
    }
}
