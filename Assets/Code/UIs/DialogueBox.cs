using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int exp;
    private bool brotherFound;
    public int killerCount;
    private Dictionary<string, List<int>> npcInteractions = new Dictionary<string, List<int>>();
    private string prevPerson = "";
    private bool attacking = false;
    public GameObject treasure;
    private bool sprzedawcaMet = false;
    public List<string> achievements = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        exp = 0;
        killerCount = 0;
        health = 10;
        brotherFound = false;
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        List<int> myList = new List<int>() {0};
        npcInteractions.Add("Sprzedawca", myList);
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0) SceneManager.LoadScene("EndScreen");
        if(!achievements.Contains("TreasureHunter") && treasure.GetComponent<Nonpickable>().treasure){
            achievements.Add("TresureHunter");
        }
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if ((interactWith != null)&&(!npcInteractions.ContainsKey(interactWith.name))) {
            List<int> myList = new List<int>() {0};
            npcInteractions.Add(interactWith.name, myList);
        }
        if(interactWith != null) {
            if((interactWith != null)&&(interactWith.name == "Bonifacy")) {
                brotherFound = true;
            }
        }
        if((interactWith != null)&&(interactWith.name == "Sprzedawca") && (brotherFound == true)&&(attacking != true)){
            achievements.Add("Reuniting family");
            string element = "findingBrother";
            ReadDialogue(element);
            brat.SetActive(false);
        }
        if (interactWith != null && interactWith.tag == "Person")
        {
            Answers(interactWith.name);
            List<int> currentStats = npcInteractions[interactWith.name];
            if(interactWith.name == "Casper") brotherFound = true;
            if(prevPerson != interactWith.name){
                currentStats = npcInteractions[interactWith.name];
                currentStats[0] = currentStats[0] + 1;
                npcInteractions[interactWith.name] = currentStats;
                prevPerson = interactWith.name;
                textComponent.text = string.Empty;
                ReadDialogue(interactWith.name);
            }
            //else ReadDialogue(interactWith.name);
        }
        if (interactWith != null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton3)))
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
    public void Heal(int value)
    {
        health += value;
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
        if((interactWith != null) && (interactWith.name == "Sprzedawca")) sprzedawcaMet = true;
        if((exp >= 3)&&(interactWith != null)&&(interactWith.name == "Sprzedawca")) {
            ReadDialogue("Sprzedawca_defeat");
            if(Input.GetKeyDown(KeyCode.JoystickButton2) || (Input.GetKeyDown(KeyCode.Keypad1)) || Input.GetKeyDown(KeyCode.Alpha1)){
                exp += 5;
                achievements.Add("Killer");
                interactWith.SetActive(false);
            }
            return;
        }
        if(!sprzedawcaMet && (interactWith != null) && (interactWith.name == "Bonifacy")){
            ReadDialogue("Bonifacy_attack");
        }
        if((interactWith != null) && (interactWith.name != "Sprzedawca") && (attacking == true)) {
            string nameCharacter = interactWith.name + "_attack";
            List<int> currentStats = npcInteractions[interactWith.name];
            currentStats[0] = -1;
            npcInteractions[interactWith.name] = currentStats;
            ReadDialogue(nameCharacter);
        }

        if((interactWith != null) && (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))){
            List<int> currentStats = npcInteractions[interactWith.name];
            if(currentStats[0] == -1){
                NextLine();
                interactWith.SetActive(false);
                health -= 1;
                exp += 1;
                killerCount += 1;
                return;
            }
            if((interactWith!= null) && ((interactWith.name == "Sprzedawca") || (interactWith.name == "Bonifacy"))) {
                string names = null;
                currentStats = npcInteractions[interactWith.name];
                currentStats.Add(1);
                for(int i = 0; i < currentStats.Count; i++){
                    names = names + currentStats[i].ToString();
                }
                if((interactWith.name == "Sprzedawca") && (names == "121")){
                    achievements.Add("Ready to Help");
                }
                if((interactWith.name == "Sprzedawca") && (names == "111" || names == "1111"))
                {
                    achievements.Add("Ready to Kill");
                    names = "111";
                    health = player.GetComponent<ProtagonistBehavior>().health - 4;
                    attacking = true;

                }
                if(names.Length >= 4) {
                    return;
                }
                if((names.Length >= 3) && (interactWith.name == "Bonifacy")) return;
                string nameCharacter = name + names;
                npcInteractions[interactWith.name] = currentStats;
                ReadDialogue(nameCharacter);
            }
        }
        if(interactWith != null && (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))){

            if((interactWith.name == "Sprzedawca") || (interactWith.name == "Bonifacy")) {
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
                ReadDialogue(nameCharacter);
                if((interactWith.name == "Sprzedawca") && (names == "112")) {
                    achievements.Add("Ready to Kill");
                    attacking = true;
                }
            }
        }
        if(interactWith != null && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))){
            if((interactWith.name == "Sprzedawca") || (interactWith.name == "Bonifacy")) {
                string names = null;
                List<int> currentStats = npcInteractions[interactWith.name];
                currentStats.Add(3);
                npcInteractions[interactWith.name] = currentStats;
                for(int i = 0; i < currentStats.Count; i++){
                    names = names + currentStats[i].ToString();
                }
                if((interactWith.name == "Sprzedawca") && (names == "121")){
                    achievements.Add("Achiever");
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
}