using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public int health;
    public TextMeshProUGUI textComponent;
    public string[] lines = new string[] { };
    public float textSpeed;
    public GameObject player;
    private string dialogue;
    private StringReader reader;
    private int index;
//  public GameObject continueButton;
    public GameObject sprzedawca;
    public GameObject brat;
    private bool first;
    private bool brotherFound;
    private Dictionary<string, List<int>> npcInteractions = new Dictionary<string, List<int>>();
    private string prevPerson = "";
    // Start is called before the first frame update
    void Start()
    {
        health = 10;
        brotherFound = false;
        textComponent.text = string.Empty;
        // StartDialogue();
        gameObject.SetActive(false);
//        continueButton.SetActive(false);
        List<int> myList = new List<int>() {0};
        npcInteractions.Add("Sprzedawca", myList);
        Debug.Log(npcInteractions["Sprzedawca"].Count);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if((interactWith != null)&&(interactWith.name == "Bonifacy")) {
            brotherFound = true;
            Debug.Log("bonifac");
        }
        if((interactWith != null)&&(interactWith.name == "Sprzedawca") && (brotherFound == true)){
            Debug.Log("FOUND");
            string element = "findingBrother";
            ReadDialogue(element);
            brat.SetActive(false);
        }
        if ((interactWith != null)&&(!npcInteractions.ContainsKey(interactWith.name))) {
            List<int> myList = new List<int>() {0};
            npcInteractions.Add(interactWith.name, myList);
        }
        if (interactWith != null && interactWith.tag == "Person")
        {
            if(interactWith.name == "Casper") brotherFound = true;
            if(prevPerson != interactWith.name){
                List<int> currentStats = npcInteractions[interactWith.name];
                Debug.Log("Current stats" + currentStats.Count);
                currentStats[0] = currentStats[0] + 1;
                npcInteractions[interactWith.name] = currentStats;
                prevPerson = interactWith.name;
                textComponent.text = string.Empty;
                ReadDialogue(interactWith.name);
            }
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
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
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
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if(Input.GetKeyDown(KeyCode.JoystickButton2) || (Input.GetKeyDown(KeyCode.Keypad1))){
            if((interactWith.name != "Sprzedawca") && (interactWith.name != "Bonifacy")) return;
            string names = null;
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats.Add(1);
            for(int i = 0; i < currentStats.Count; i++){
                names = names + currentStats[i].ToString();
            }
            if((interactWith.name == "Sprzedawca") && (names == "111" || names == "1111"))
            {
                names = "111";
                health = player.GetComponent<ProtagonistBehavior>().health - 5;
                Debug.Log("Dialogue box health " + health);
            }
            if(names.Length >= 4) {
                return;
            }
            if((names.Length >= 3) && (interactWith.name == "Bonifacy")) return;
            string nameCharacter = name + names;
            npcInteractions[interactWith.name] = currentStats;
            ReadDialogue(nameCharacter);
        }
        if(Input.GetKeyDown(KeyCode.JoystickButton1) || (Input.GetKeyDown(KeyCode.Keypad2))){
            string names = null;
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats.Add(2);
            npcInteractions[interactWith.name] = currentStats;
            for(int i = 0; i < currentStats.Count; i++){
                names = names + currentStats[i].ToString();
            }
            if(names.Length >= 4) {
                return;
            }
            if((names == "122") && (interactWith.name == "Sprzedawca")) return;
            string nameCharacter = name + names;
            npcInteractions[interactWith.name] = currentStats;
            Debug.Log("name" + name);
            Debug.Log("names" + names);
            ReadDialogue(nameCharacter);
        }
        if(Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Keypad3))){
            string names = null;
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats.Add(3);
            npcInteractions[interactWith.name] = currentStats;
            for(int i = 0; i < currentStats.Count; i++){
                names = names + currentStats[i].ToString();
            }
            if(names == "13") {
            List<int> emptyList = new List<int>() {0};
            npcInteractions[interactWith.name] = emptyList;
            }
            if((names.Length <= 2) || (names.Length >= 4)) return;
            string nameCharacter = name + names;
            npcInteractions[interactWith.name] = currentStats;
            ReadDialogue(nameCharacter);
        }
    }
}