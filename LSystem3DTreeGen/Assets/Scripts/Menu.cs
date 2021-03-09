using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject TreeVariables;
    public GameObject player;
    public GameObject UICamera;
    public InputField Rule;
    public Dropdown character;
    public Button returnButton;
    char currentCharacter = 'A';
    // Start is called before the first frame update
    void Start()
    {
        Rule.text = Settings.alphabetNonParametric[currentCharacter];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.SetActive(false);
            UICamera.SetActive(true);
            TreeVariables.SetActive(true);
            character.enabled = true;
            returnButton.enabled = true;
            Rule.enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void setAlphabet(int option)
    {
        if(option == 0)
        {
            Rule.text = Settings.alphabetNonParametric['A'];
            currentCharacter = 'A';
        }
        if (option == 1)
        {
            Rule.text = Settings.alphabetNonParametric['F'];
            currentCharacter = 'F';
        }
        if (option == 2)
        {
            Rule.text = Settings.alphabetNonParametric['S'];
            currentCharacter = 'S';
        }
        if (option == 3)
        {
            Rule.text = Settings.alphabetNonParametric['L'];
            currentCharacter = 'L';
        }
    }
    public void setRule(string rule)
    {
        Settings.alphabetNonParametric[currentCharacter] = rule;
    }
    public void setInitial(string initial)
    {
        Settings.initial = initial;
    }
    public void setFall(bool f)
    {
        Settings.fall = f;
    }
    public void setWidthRatio(float widthRatio)
    {
        Settings.width_ratio = widthRatio;
    }
    public void setLineLength(float lineLength)
    {
        Settings.line_length = lineLength;
    }
    public void setDelta(float delta)
    {
        Settings.delta = delta;
    }
    public void setDeltaRange(float deltaRange)
    {
        Settings.delta_range = deltaRange;
    }
    public void setTrunkSize(float trunkSize)
    {
        Settings.trunk_size = trunkSize;
    }
    public void setLeafSize(float leafSize)
    {
        Settings.leaf_size = leafSize;
    }
    public void returnToGame()
    {
        Cursor.visible = false;
        player.SetActive(true);
        UICamera.SetActive(false);
        TreeVariables.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        character.enabled = false;
        returnButton.enabled = false;
        Rule.enabled = false;

    }
}
