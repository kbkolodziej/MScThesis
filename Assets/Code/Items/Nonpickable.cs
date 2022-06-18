using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Nonpickable : MonoBehaviour
{
    private string dialogue;
    private StringReader reader;
    private string[] information;
    public bool treasure;
    public bool studnia;
    // Start is called before the first frame update
    void Start()
    {
        GetInformationAboutMe();
        treasure = false;
        studnia = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static string ReadInformations(string name)
    {
        StreamReader oStreamReader = new StreamReader(Path.Combine("Assets/Dialogues/Nonpickables/" + name + ".txt"));
        string dialogue = oStreamReader.ReadToEnd();
        oStreamReader.Close();
        return dialogue;
    }

    private void GetInformationAboutMe()
    {
        dialogue = ReadInformations(name);
        reader = new StringReader(dialogue);
        int iteration = 0;
        string line = "";
        List<string> newDialogue = new List<string>();
        bool found = false;
        while (!found)
        {
            line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                found = true;
                break;
            }
            newDialogue.Add(line);
            iteration++;
        }
        information = newDialogue.ToArray();
    }

    public string[] GetMyInfo()
    {
        if (name == "Treasure")
        {
            treasure = true;
        }
        if (name == "GlebokaStudnia")
        {
            studnia = true;
        }
        return information;
    }
}
