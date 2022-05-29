using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines = new string[] { };
    public float textSpeed;
    public GameObject player;
    private string dialogue;
    private StringReader reader;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        // StartDialogue();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if (interactWith != null && interactWith.tag == "Person")
        {
            ReadDialogue(interactWith.name);
        }
        if ((Input.GetMouseButtonDown(0)) || (Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if (textComponent.text == lines[index])
                NextLine();
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                interactWith.GetComponent<NPCBehavior>().isTalking = false;
            }
        }
    }

    void ReadDialogue(string name)
    {
        dialogue = ReadingDialogues.ReadDialogue(name);
        Debug.Log(dialogue);
        reader = new StringReader(dialogue);
        string line = "";
        string lineWithAnswers = "";
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
            if (line[0] == 'j')
            {
                lineWithAnswers = line.Substring(3) + System.Environment.NewLine;
                line = reader.ReadLine();
                bool isDialogueOption = true;
                int number = 1;
                while(isDialogueOption) 
                {
                    lineWithAnswers = lineWithAnswers.Insert(lineWithAnswers.Length, 
                        number +  ". " + line.Substring(5) + System.Environment.NewLine);
                    line = reader.ReadLine();
                    if(line[0] != 'x')
                        isDialogueOption = false;
                    number++;
                }
                newDialogue.Add(lineWithAnswers);
            }
        }
        lines = newDialogue.ToArray();
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); // change to wait for input
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            gameObject.SetActive(false);
        }
    }

}
