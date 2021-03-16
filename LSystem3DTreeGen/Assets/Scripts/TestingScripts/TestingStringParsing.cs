using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingStringParsing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //MAKE A DICTIONARY OF FLOATS CORRESPONDING TO PARAMETER NAMES
        string[] rules = "!(w).F(l).[.&(a0).B(l* r2, w* wr).]./(d).A(l* r1, w* wr)".Split('.');
        string lambdaOne = ""; 
        string lambdaTwo = "";
        foreach (string word in rules)
        {
            string action = word[0].ToString();
            //IF ACTION HAS PARAMETER
            if(word.Length > 1 && word[1] == '(')
            {
                //IF THERE ARE TWO PARAMETERS
                if (word.Contains(","))
                {

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
