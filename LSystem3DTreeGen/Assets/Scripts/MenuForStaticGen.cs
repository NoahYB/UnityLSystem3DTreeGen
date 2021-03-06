using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuForStaticGen : MonoBehaviour
{
    Dictionary<(string, bool), List<Module>> alphabet;
    List<Module> initial;
    int generations;

    List<Module> initialRand;
    Dictionary<(string, bool), List<Module>> alphabetRand;
    CreateRandomLSystem CRLS;
    // Start is called before the first frame update
    void Start()
    {
        CRLS = new CreateRandomLSystem();
    }
    public void SetGenerations(string gens)
    {
        generations = int.Parse(gens);
        
       
    }
    public void ExportObj()
    {
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricAnimated>().ExportObj("ExportedObj");
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
            alphabet = Settings.moduleAlphabetForSpaceFillingLine2;
            initial = Settings.initialModuleForSpaceFillingLine2;
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
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricGenerator>().Init(alphabet, initial, generations);
    }
    public void GenerateRandom()
    {
        initialRand = CRLS.CreateRandomAxiom();
        alphabetRand = CRLS.CreateRandomAlphabet();
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricGenerator>().Init(alphabetRand, initialRand, generations);
    }

    public void RegenerateRandom()
    {
        GameObject.FindGameObjectWithTag("Generator").GetComponent<ParametricGenerator>().Init(alphabetRand, initialRand, generations);
    }
}
