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
//  public GameObject continueButton;
    private bool first;
    private Dictionary<string, List<int>> npcInteraction = new Dictionary<string, List<int>>();
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        // StartDialogue();
        gameObject.SetActive(false);
//        continueButton.SetActive(false);
        npcInteractions.Add("Sprzedawca", 0);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if (interactWith != null && interactWith.tag == "Person")
        {
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats[0] = currentStats[0] + 1;
            npcInteractions[interactWith.name = currentStats[0]];
            if(first) Answers(interactWith.name);
            else ReadDialogue(interactWith.name);
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton3))
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
        first = true;
//        continueButton.SetActive(true);
        dialogue = ReadingDialogues.ReadDialogue(name);
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
                Debug.Log(index);
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

    public void Answers(string name){
        if(Input.GetKeyDown(KeyCode.JoystickButton2) || (Input.GetKeyDown(KeyCode.Keypad1))){
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats[1] = 1;
            npcInteractions[interactWith.name = currentStats[1]];
            name = name+"1";
            ReadDialogue(name);
        }
        if(Input.GetKeyDown(KeyCode.JoystickButton1) || (Input.GetKeyDown(KeyCode.Keypad2))){
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats[1] = 2;
            npcInteractions[interactWith.name = currentStats[1]];
            name = name+"2";
            ReadDialogue(name);
        }
        if(Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Keypad3))){
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats[1] = 3;
            npcInteractions[interactWith.name = currentStats[1]];
            name = name+"3";
            ReadDialogue(name);
        }
    }
}