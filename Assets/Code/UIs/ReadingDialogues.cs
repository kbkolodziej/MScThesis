using System.IO;
using UnityEngine;
public class ReadingDialogues
{
    public static string ReadDialogue(string name)
    {
        StreamReader oStreamReader = new StreamReader(Path.Combine("Assets/Dialogues/People/" + name + ".txt"));
        string dialogue = oStreamReader.ReadToEnd();
        oStreamReader.Close();
        return dialogue;
    }

}
